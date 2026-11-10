/* Giáo án AI (Member/Plan)
   - Trang Create: gửi khảo sát (AJAX), màn hình chờ, chèn kết quả (HTML do server render),
     tạo lại toàn bộ / chỉ thực đơn / một ngày, lưu giáo án.
   - Trang Today: tích "Đã hoàn thành" từng bài. */
(function () {
  "use strict";

  function token(root) {
    var input = (root || document).querySelector('input[name="__RequestVerificationToken"]');
    return input ? input.value : "";
  }

  function post(url, data, root) {
    var body = data instanceof FormData ? data : new FormData();
    if (!(data instanceof FormData)) {
      Object.keys(data || {}).forEach(function (k) {
        if (data[k] !== undefined && data[k] !== null) body.append(k, data[k]);
      });
    }
    if (!body.has("__RequestVerificationToken")) body.append("__RequestVerificationToken", token(root));
    return fetch(url, {
      method: "POST",
      body: body,
      credentials: "same-origin",
      headers: { "X-Requested-With": "XMLHttpRequest" }
    }).then(function (res) {
      var type = res.headers.get("content-type") || "";
      if (type.indexOf("application/json") !== -1) {
        return res.json().then(function (json) { return { ok: res.ok, status: res.status, json: json }; });
      }
      return res.text().then(function (text) { return { ok: res.ok, status: res.status, html: text }; });
    });
  }

  /* ================= Trang tạo giáo án ================= */
  var create = document.getElementById("apCreate");
  if (create) {
    var form = document.getElementById("apForm");
    var loading = document.getElementById("apLoading");
    var result = document.getElementById("apResult");
    var editAgain = document.getElementById("apEditAgain");
    var formError = document.getElementById("apFormError");
    var submitBtn = document.getElementById("apSubmit");
    var quota = document.getElementById("apQuota");
    var base = form.getAttribute("action").replace(/\/Generate$/i, "");
    var phaseTimer = null;
    var busy = false;

    var setStep = function (n) {
      create.querySelectorAll(".ap-steps li").forEach(function (li) {
        var s = +li.getAttribute("data-step");
        li.classList.toggle("is-current", s === n);
        li.classList.toggle("is-done", s < n);
      });
    };

    var startLoading = function () {
      loading.hidden = false;
      var items = loading.querySelectorAll("li");
      var i = 0;
      items.forEach(function (li) { li.className = ""; });
      items[0].className = "is-active";
      clearInterval(phaseTimer);
      phaseTimer = setInterval(function () {
        if (i < items.length - 1) {
          items[i].className = "is-done";
          i++;
          items[i].className = "is-active";
        }
      }, 6000);
      loading.scrollIntoView({ behavior: "smooth", block: "center" });
    };

    var stopLoading = function () {
      clearInterval(phaseTimer);
      loading.hidden = true;
    };

    var clearErrors = function () {
      formError.hidden = true;
      formError.textContent = "";
      form.querySelectorAll(".ap-field").forEach(function (f) {
        f.classList.remove("is-invalid");
        var e = f.querySelector(".ap-err");
        if (e) e.textContent = "";
      });
    };

    var showErrors = function (message, errors) {
      formError.textContent = message || "Vui lòng kiểm tra lại thông tin.";
      formError.hidden = false;
      Object.keys(errors || {}).forEach(function (key) {
        var name = key.split(".").pop();
        var field = form.querySelector('.ap-field[data-field="' + name + '"]');
        if (!field) return;
        field.classList.add("is-invalid");
        var e = field.querySelector(".ap-err");
        if (e) e.textContent = (errors[key] || [])[0] || "";
      });
      var first = form.querySelector(".ap-field.is-invalid") || formError;
      first.scrollIntoView({ behavior: "smooth", block: "center" });
    };

    var clientValidate = function () {
      var errors = {};
      var sessions = +(form.querySelector('input[name="SessionsPerWeek"]:checked') || {}).value || 3;
      var days = form.querySelectorAll('input[name="Weekdays"]:checked').length;
      if (days < sessions) errors.Weekdays = ["Hãy chọn ít nhất " + sessions + " ngày có thể tập trong tuần."];
      if (!form.querySelector('input[name="Gender"]:checked')) errors.Gender = ["Vui lòng chọn giới tính"];
      ["HeightCm", "WeightKg", "DateOfBirth"].forEach(function (n) {
        var el = form.elements[n];
        if (el && !el.value) errors[n] = ["Vui lòng nhập thông tin này"];
      });
      if (!form.elements["Consent"].checked) errors.Consent = ["Vui lòng đồng ý gửi số đo cho AI để tạo gợi ý."];
      return errors;
    };

    var showResult = function (html) {
      result.innerHTML = html;
      var inner = document.getElementById("apResultInner");
      if (inner && quota) quota.textContent = inner.getAttribute("data-remaining");
      editAgain.hidden = false;
      setStep(3);
      result.scrollIntoView({ behavior: "smooth", block: "start" });
    };

    var handleFailure = function (r, backToForm) {
      var msg = (r.json && r.json.message) || "Có lỗi xảy ra, vui lòng thử lại.";
      if (backToForm) {
        form.hidden = false;
        result.hidden = false;
        setStep(1);
        showErrors(msg, r.json && r.json.errors);
      } else {
        result.hidden = false;
        alertInResult(msg);
      }
    };

    var alertInResult = function (msg) {
      var box = document.createElement("div");
      box.className = "ap-notice ap-notice--error";
      box.setAttribute("role", "alert");
      box.textContent = msg;
      result.insertBefore(box, result.firstChild);
      box.scrollIntoView({ behavior: "smooth", block: "center" });
    };

    form.addEventListener("submit", function (ev) {
      ev.preventDefault();
      if (busy) return;
      clearErrors();
      var errors = clientValidate();
      if (Object.keys(errors).length) {
        showErrors("Vui lòng kiểm tra lại các mục được đánh dấu.", errors);
        return;
      }
      busy = true;
      submitBtn.disabled = true;
      form.hidden = true;
      result.innerHTML = "";
      editAgain.hidden = true;
      setStep(2);
      startLoading();

      post(form.action, new FormData(form), form)
        .then(function (r) {
          stopLoading();
          if (r.ok && r.html !== undefined) return showResult(r.html);
          handleFailure(r, true);
        })
        .catch(function () {
          stopLoading();
          handleFailure({ json: { message: "Mất kết nối tới máy chủ, vui lòng thử lại." } }, true);
        })
        .finally(function () {
          busy = false;
          submitBtn.disabled = false;
        });
    });

    document.getElementById("apBackToForm").addEventListener("click", function () {
      form.hidden = false;
      result.innerHTML = "";
      editAgain.hidden = true;
      setStep(1);
      form.scrollIntoView({ behavior: "smooth", block: "start" });
    });

    // Các nút trong phần kết quả (được chèn sau nên dùng uỷ quyền sự kiện)
    result.addEventListener("click", function (ev) {
      var toggle = ev.target.closest("#apRegenToggle");
      if (toggle) {
        var menu = document.getElementById("apRegenMenu");
        menu.hidden = !menu.hidden;
        toggle.setAttribute("aria-expanded", String(!menu.hidden));
        return;
      }

      var regen = ev.target.closest("[data-regen]");
      if (regen && !busy) {
        var inner = document.getElementById("apResultInner");
        var reason = result.querySelector('input[name="apReason"]:checked');
        busy = true;
        result.hidden = true;
        editAgain.hidden = true;
        setStep(2);
        startLoading();
        post(base + "/Regenerate", {
          draftId: inner.getAttribute("data-draft-id"),
          part: regen.getAttribute("data-regen"),
          weekday: regen.getAttribute("data-weekday"),
          reason: reason ? reason.value : ""
        }, form)
          .then(function (r) {
            stopLoading();
            result.hidden = false;
            editAgain.hidden = false;
            if (r.ok && r.html !== undefined) return showResult(r.html);
            setStep(3);
            handleFailure(r, false);
          })
          .catch(function () {
            stopLoading();
            result.hidden = false;
            setStep(3);
            alertInResult("Mất kết nối tới máy chủ, vui lòng thử lại.");
          })
          .finally(function () { busy = false; });
        return;
      }

      var save = ev.target.closest("#apSave");
      if (save && !busy) {
        doSave(save, false);
      }
    });

    var doSave = function (btn, replace) {
      var inner = document.getElementById("apResultInner");
      busy = true;
      btn.disabled = true;
      btn.textContent = "Đang lưu…";
      setStep(4);
      post(base + "/Save", { draftId: inner.getAttribute("data-draft-id"), replaceTrainerPlan: replace ? "true" : "false" }, form)
        .then(function (r) {
          if (r.ok && r.json && r.json.success) {
            window.location.href = r.json.redirectUrl || base;
            return;
          }
          if (r.status === 409 && r.json && r.json.needConfirm) {
            busy = false;
            btn.disabled = false;
            btn.textContent = "Lưu vào Giáo án của tôi";
            if (window.confirm(r.json.message)) doSave(btn, true);
            else setStep(3);
            return;
          }
          busy = false;
          btn.disabled = false;
          btn.textContent = "Lưu vào Giáo án của tôi";
          setStep(3);
          alertInResult((r.json && r.json.message) || "Không lưu được, vui lòng thử lại.");
        })
        .catch(function () {
          busy = false;
          btn.disabled = false;
          btn.textContent = "Lưu vào Giáo án của tôi";
          setStep(3);
          alertInResult("Mất kết nối tới máy chủ, vui lòng thử lại.");
        });
    };
  }

  /* ================= Trang hôm nay ================= */
  var today = document.getElementById("apToday");
  if (today) {
    var list = today.querySelector(".ap-check-list");
    if (!list) return;
    var total = +list.getAttribute("data-total") || 0;
    var countEl = document.getElementById("apDoneCount");
    var bar = document.getElementById("apProgress");
    var banner = document.getElementById("apDoneBanner");
    var error = document.getElementById("apTodayError");
    var url = window.location.pathname.replace(/\/Today\/?$/i, "") + "/ToggleLog";

    var render = function (doneCount) {
      countEl.textContent = doneCount;
      bar.style.width = (total ? Math.round(doneCount * 100 / total) : 0) + "%";
      banner.hidden = !(total > 0 && doneCount >= total);
    };

    list.addEventListener("change", function (ev) {
      var box = ev.target;
      if (!box.matches('input[type="checkbox"][data-exercise]')) return;
      var item = box.closest(".ap-check");
      var done = box.checked;
      item.classList.toggle("is-done", done);
      box.disabled = true;
      error.hidden = true;
      post(url, { exerciseId: box.getAttribute("data-exercise"), done: done ? "true" : "false" }, today)
        .then(function (r) {
          if (r.ok && r.json && r.json.success) {
            render(r.json.doneCount);
            return;
          }
          throw new Error((r.json && r.json.message) || "Không lưu được, vui lòng thử lại.");
        })
        .catch(function (err) {
          box.checked = !done;
          item.classList.toggle("is-done", !done);
          error.textContent = err.message || "Không lưu được, vui lòng thử lại.";
          error.hidden = false;
        })
        .finally(function () { box.disabled = false; });
    });
  }
})();
