/* Bảng hồ sơ cá nhân của HLV (Area PT).
   Mở: bấm ảnh đại diện ở góc dưới menu (data-profile-open) hoặc URL có ?profile=open.
   Lưu: gửi form bằng fetch tới /PT/Profile/Update, không chuyển trang; cập nhật luôn tên/ảnh trên menu. */
(function () {
  "use strict";

  var drawer = document.getElementById("cxProfile");
  if (!drawer) return;

  var panel = drawer.querySelector(".cx-drawer-panel");
  var form = document.getElementById("cxProfileForm");
  var notice = document.getElementById("cxProfileNotice");
  var saveBtn = document.getElementById("cxProfileSave");
  var openers = document.querySelectorAll("[data-profile-open]");
  var lastFocus = null;
  var dirty = false;

  var EMAIL_RE = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  var PHONE_RE = /^\+?[\d\s().\-]{8,20}$/;
  var RULES = {
    FullName: function (v) { if (!v.trim()) return "Vui lòng nhập họ và tên"; },
    Email: function (v) { if (!v.trim()) return "Vui lòng nhập email"; if (!EMAIL_RE.test(v.trim())) return "Email không hợp lệ"; },
    PhoneNumber: function (v) { if (v.trim() && !PHONE_RE.test(v.trim())) return "Số điện thoại không hợp lệ"; },
    Specialization: function (v) { if (!v.trim()) return "Vui lòng nhập chuyên môn"; },
    AvatarUrl: function (v) { if (v.trim() && !/^https?:\/\/\S+$/i.test(v.trim())) return "Đường dẫn ảnh phải bắt đầu bằng http:// hoặc https://"; }
  };

  /* ---------- mở / đóng ---------- */
  function open() {
    lastFocus = document.activeElement;
    document.body.classList.remove("cx-nav-open"); // đang mở menu trên điện thoại thì đóng lại
    var scrim = document.querySelector(".cx-scrim"); if (scrim) scrim.hidden = true;
    drawer.classList.add("is-open");
    drawer.setAttribute("aria-hidden", "false");
    document.body.style.overflow = "hidden";
    openers.forEach(function (b) { b.setAttribute("aria-expanded", "true"); });
    setTimeout(function () { form.elements["FullName"].focus({ preventScroll: true }); }, 80);
  }
  function close() {
    if (!drawer.classList.contains("is-open")) return;
    drawer.classList.remove("is-open");
    drawer.setAttribute("aria-hidden", "true");
    document.body.style.overflow = "";
    openers.forEach(function (b) { b.setAttribute("aria-expanded", "false"); });
    if (lastFocus && lastFocus.focus) lastFocus.focus({ preventScroll: true });
  }

  openers.forEach(function (b) { b.addEventListener("click", open); });
  drawer.addEventListener("click", function (e) { if (e.target.closest("[data-profile-close]")) close(); });

  document.addEventListener("keydown", function (e) {
    if (!drawer.classList.contains("is-open")) return;
    if (e.key === "Escape") { e.preventDefault(); close(); return; }
    if (e.key !== "Tab") return;
    var items = [].filter.call(panel.querySelectorAll("button,input,textarea,a[href]"), function (el) { return !el.disabled && el.offsetParent !== null; });
    if (!items.length) return;
    var first = items[0], last = items[items.length - 1];
    if (e.shiftKey && document.activeElement === first) { e.preventDefault(); last.focus(); }
    else if (!e.shiftKey && document.activeElement === last) { e.preventDefault(); first.focus(); }
  });

  /* ---------- xem trước trực tiếp ---------- */
  var pName = drawer.querySelector("[data-preview-name]");
  var pSpec = drawer.querySelector("[data-preview-spec]");
  var pPhoto = drawer.querySelector("[data-preview-photo]");
  var bioCount = drawer.querySelector("[data-bio-count]");

  function initials(name) {
    var w = (name || "").trim().split(/\s+/).filter(Boolean);
    if (!w.length) return "?";
    return (w.length === 1 ? w[0][0] : w[w.length - 2][0] + w[w.length - 1][0]).toUpperCase();
  }
  function setPhoto(el, url, name, tag) {
    el.innerHTML = "";
    if (url && /^https?:\/\//i.test(url)) {
      var img = document.createElement("img");
      img.alt = ""; img.src = url;
      img.onerror = function () { setPhoto(el, "", name, tag); };
      el.appendChild(img);
    } else if (tag === "text") {
      el.textContent = initials(name);
    } else {
      var b = document.createElement("b");
      b.textContent = initials(name);
      el.appendChild(b);
    }
  }
  function refreshPreview() {
    pName.textContent = form.elements["FullName"].value.trim() || "Họ và tên";
    pSpec.textContent = form.elements["Specialization"].value.trim() || "Chuyên môn";
    bioCount.textContent = form.elements["Bio"].value.length;
  }
  var avatarTimer = null;
  form.addEventListener("input", function (e) {
    dirty = true;
    refreshPreview();
    if (e.target.name === "AvatarUrl") {
      clearTimeout(avatarTimer);
      avatarTimer = setTimeout(function () { setPhoto(pPhoto, e.target.value.trim(), form.elements["FullName"].value); }, 400);
    }
    var f = e.target.closest(".cx-field");
    if (f && f.classList.contains("is-invalid")) validateField(e.target.name);
  });

  /* ---------- kiểm tra & gửi ---------- */
  function errEl(name) { return form.querySelector('[data-err-for="' + name + '"]'); }
  function setError(name, msg) {
    var p = errEl(name), input = form.elements[name];
    if (!p || !input) return false;
    p.textContent = msg || "";
    input.closest(".cx-field").classList.toggle("is-invalid", !!msg);
    if (msg) input.setAttribute("aria-invalid", "true"); else input.removeAttribute("aria-invalid");
    return true;
  }
  function validateField(name) {
    var msg = RULES[name] ? RULES[name](form.elements[name].value) : "";
    setError(name, msg);
    return !msg;
  }
  function showNotice(type, msg) {
    notice.className = "cx-drawer-notice is-" + type;
    notice.textContent = msg;
    notice.hidden = false;
  }

  form.addEventListener("submit", function (e) {
    e.preventDefault();
    notice.hidden = true;
    var firstBad = null;
    Object.keys(RULES).forEach(function (n) { if (!validateField(n) && !firstBad) firstBad = form.elements[n]; });
    if (firstBad) { firstBad.focus(); return; }

    saveBtn.disabled = true;
    saveBtn.textContent = "Đang lưu…";

    fetch(form.action, {
      method: "POST",
      body: new FormData(form),
      credentials: "same-origin",
      headers: { "X-Requested-With": "XMLHttpRequest", "Accept": "application/json" }
    })
      .then(function (res) { return res.json().catch(function () { return null; }); })
      .then(function (data) {
        if (data && data.success) {
          dirty = false;
          showNotice("ok", data.message);
          var p = data.profile || {};
          // cập nhật thẻ ở góc dưới menu
          var n = document.querySelector("[data-profile-name]");
          var s = document.querySelector("[data-profile-spec]");
          var a = document.querySelector("[data-profile-avatar]");
          if (n) n.textContent = p.fullName;
          if (s) s.textContent = p.specialization;
          if (a) setPhoto(a, p.avatarUrl, p.fullName, "text");
          setPhoto(pPhoto, p.avatarUrl, p.fullName);
          return;
        }
        var errors = (data && data.errors) || {};
        var focused = false, general = null;
        Object.keys(errors).forEach(function (k) {
          var msg = errors[k] && errors[k][0];
          if (k && setError(k, msg)) { if (!focused) { form.elements[k].focus(); focused = true; } }
          else if (msg) general = msg;
        });
        showNotice("err", general || (data && data.message) || "Chưa lưu được. Vui lòng thử lại.");
      })
      .catch(function () { showNotice("err", "Không kết nối được máy chủ. Kiểm tra mạng rồi thử lại."); })
      .then(function () { saveBtn.disabled = false; saveBtn.textContent = "Lưu hồ sơ"; });
  });

  window.addEventListener("beforeunload", function (e) {
    if (dirty && drawer.classList.contains("is-open")) { e.preventDefault(); e.returnValue = ""; }
  });

  // /PT/Profile hoặc ?profile=open → mở sẵn
  try {
    var u = new URL(window.location.href);
    if (u.searchParams.get("profile") === "open") {
      open();
      u.searchParams.delete("profile");
      history.replaceState(null, "", u.pathname + u.search + u.hash);
    }
  } catch (err) { /* trình duyệt cũ */ }
})();
