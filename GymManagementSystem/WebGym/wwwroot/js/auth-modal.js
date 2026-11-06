/* Modal đăng nhập / đăng ký / quên mật khẩu — gửi form bằng fetch, không chuyển trang.
   Mở bằng: <a data-auth-open="login|register|forgot" href="/Auth/Login">  (href là fallback khi tắt JS)
   API: window.AuthModal.open('login'|'register'|'forgot'), window.AuthModal.close()
   Mở sẵn bằng URL: ?auth=login | ?auth=register | ?auth=forgot | ?auth=reset&token=...
   (liên kết trong email đặt lại mật khẩu dẫn về ?auth=reset&token=...) */
(function () {
  "use strict";

  var modal = document.getElementById("authModal");
  if (!modal) return; // đã đăng nhập → partial không render

  var box = modal.querySelector(".am-box");
  var tabsBar = modal.querySelector(".am-tabs");
  var tabBtns = [].slice.call(modal.querySelectorAll(".am-tab"));
  var notice = document.getElementById("amNotice");
  var titleEl = document.getElementById("amTitle");
  var subEl = document.getElementById("amSub");
  var devLinkBox = document.getElementById("amDevLink");
  var devLinkA = document.getElementById("amDevLinkA");
  var forms = {
    login: document.getElementById("amLogin"),
    register: document.getElementById("amRegister"),
    forgot: document.getElementById("amForgot"),
    reset: document.getElementById("amReset")
  };
  var TABS = ["login", "register"]; // 2 tab hiển thị; forgot/reset là màn hình phụ (ẩn thanh tab)

  var COPY = {
    login: { title: "Chào mừng trở lại", sub: "Đăng nhập để tiếp tục hành trình tập luyện." },
    register: { title: "Tạo tài khoản mới", sub: "Trở thành hội viên của Pulse Gym để đặt lịch với HLV và theo dõi tiến độ." },
    forgot: { title: "Quên mật khẩu", sub: "Nhập email đã đăng ký, chúng tôi sẽ gửi liên kết để đặt lại mật khẩu." },
    reset: { title: "Đặt mật khẩu mới", sub: "Tạo mật khẩu mới cho tài khoản Pulse Gym của bạn." }
  };

  var current = "login";
  var lastFocus = null;
  var redirecting = false;

  /* ---------------- Validation phía client (khớp DataAnnotations) ---------------- */
  var EMAIL_RE = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  var PHONE_RE = /^\+?[\d\s().\-]{8,20}$/;

  var RULES = {
    login: {
      Email: function (v) { if (!v) return "Vui lòng nhập Email"; if (!EMAIL_RE.test(v)) return "Email không đúng định dạng"; },
      Password: function (v) { if (!v) return "Vui lòng nhập mật khẩu"; }
    },
    register: {
      FullName: function (v) { if (!v.trim()) return "Họ và tên không được để trống"; if (v.length > 100) return "Tối đa 100 ký tự"; },
      Email: function (v) { if (!v) return "Vui lòng nhập Email"; if (!EMAIL_RE.test(v)) return "Email không hợp lệ"; },
      PhoneNumber: function (v) { if (!v.trim()) return "Vui lòng nhập số điện thoại"; if (!PHONE_RE.test(v.trim())) return "Số điện thoại không hợp lệ"; },
      Password: function (v) { if (!v) return "Vui lòng nhập mật khẩu"; if (v.length < 6) return "Mật khẩu tối thiểu 6 ký tự"; },
      ConfirmPassword: confirmRule
    },
    forgot: {
      Email: function (v) { if (!v) return "Vui lòng nhập Email"; if (!EMAIL_RE.test(v)) return "Email không hợp lệ"; }
    },
    reset: {
      Password: function (v) { if (!v) return "Vui lòng nhập mật khẩu mới"; if (v.length < 6) return "Mật khẩu tối thiểu 6 ký tự"; if (v.length > 100) return "Tối đa 100 ký tự"; },
      ConfirmPassword: confirmRule
    }
  };

  function confirmRule(v, form) {
    if (!v) return "Vui lòng xác nhận mật khẩu";
    if (v !== form.elements["Password"].value) return "Mật khẩu xác nhận không khớp";
  }

  function formKey(form) {
    for (var k in forms) { if (forms[k] === form) return k; }
    return "login";
  }
  function fieldOf(form, name) {
    var input = form.elements[name];
    return input && input.closest ? input.closest(".am-field") : null;
  }
  function setError(form, name, msg) {
    var f = fieldOf(form, name);
    if (!f) return false;
    var p = f.querySelector(".am-err");
    if (msg) { p.textContent = msg; f.classList.add("is-invalid"); form.elements[name].setAttribute("aria-invalid", "true"); }
    else { p.textContent = ""; f.classList.remove("is-invalid"); form.elements[name].removeAttribute("aria-invalid"); }
    return true;
  }
  function clearErrors(form) {
    [].forEach.call(form.querySelectorAll(".am-field"), function (f) {
      f.classList.remove("is-invalid");
      f.querySelector(".am-err").textContent = "";
    });
    [].forEach.call(form.querySelectorAll("[aria-invalid]"), function (i) { i.removeAttribute("aria-invalid"); });
  }
  function validateField(form, name) {
    var rule = RULES[formKey(form)][name];
    if (!rule) return true;
    var msg = rule(form.elements[name].value, form);
    setError(form, name, msg);
    return !msg;
  }
  function validate(form) {
    var first = null;
    Object.keys(RULES[formKey(form)]).forEach(function (name) {
      if (!validateField(form, name) && !first) first = form.elements[name];
    });
    if (first) first.focus();
    return !first;
  }

  /* ---------------- Thông báo ---------------- */
  function showNotice(type, msg) {
    notice.className = "am-notice is-" + type;
    notice.textContent = msg;
    notice.hidden = false;
  }
  function hideNotice() { notice.hidden = true; notice.textContent = ""; }

  // Chỉ hiện ở môi trường Development (server mới trả devLink)
  function showDevLink(url) {
    if (!devLinkBox || !url) return;
    devLinkA.href = url;
    devLinkBox.hidden = false;
  }
  function hideDevLink() { if (devLinkBox) devLinkBox.hidden = true; }

  /* ---------------- Mở / đóng ---------------- */
  function scrollbarWidth() { return window.innerWidth - document.documentElement.clientWidth; }

  function open(tab) {
    lastFocus = document.activeElement;
    switchTab(tab || "login", false);
    var sw = scrollbarWidth();
    document.body.style.overflow = "hidden";
    if (sw > 0) document.body.style.paddingRight = sw + "px";
    modal.classList.add("open");
    modal.setAttribute("aria-hidden", "false");
    setTimeout(function () {
      var first = forms[current].querySelector("input:not([type=hidden]):not([type=checkbox])");
      if (first) first.focus({ preventScroll: true });
    }, 60);
  }

  function close() {
    if (!modal.classList.contains("open")) return;
    modal.classList.remove("open");
    modal.setAttribute("aria-hidden", "true");
    document.body.style.overflow = "";
    document.body.style.paddingRight = "";
    if (lastFocus && lastFocus.focus) lastFocus.focus({ preventScroll: true });
  }

  function switchTab(tab, focus) {
    if (!forms[tab]) return;
    current = tab;
    modal.dataset.tab = tab;
    if (TABS.indexOf(tab) !== -1) {
      tabsBar.dataset.active = tab;
      tabBtns.forEach(function (b) {
        var on = b.dataset.amTab === tab;
        b.classList.toggle("is-on", on);
        b.setAttribute("aria-selected", on ? "true" : "false");
        b.tabIndex = on ? 0 : -1;
      });
    }
    Object.keys(forms).forEach(function (k) { forms[k].hidden = k !== tab; });
    titleEl.textContent = COPY[tab].title;
    subEl.textContent = COPY[tab].sub;
    hideNotice();
    hideDevLink();
    clearErrors(forms[tab]);
    if (focus) {
      var first = forms[tab].querySelector("input:not([type=hidden]):not([type=checkbox])");
      if (first) first.focus({ preventScroll: true });
    }
  }

  /* ---------------- Gửi form ---------------- */
  function setBusy(form, busy) {
    var btn = form.querySelector(".am-submit");
    btn.classList.toggle("is-busy", busy);
    btn.disabled = busy;
    btn.setAttribute("aria-busy", busy ? "true" : "false");
  }

  function applyServerErrors(form, data) {
    // Liên kết đặt lại mật khẩu hết hạn / đã dùng → quay về màn hình quên mật khẩu
    if (data && data.expired) {
      forms.reset.reset();
      switchTab("forgot", true);
      showNotice("error", data.message || "Liên kết đặt lại mật khẩu không còn hiệu lực.");
      return;
    }
    var errors = (data && data.errors) || {};
    var general = [];
    var firstField = null;
    Object.keys(errors).forEach(function (key) {
      var msg = errors[key] && errors[key][0];
      if (!msg) return;
      if (key && setError(form, key, msg)) { if (!firstField) firstField = form.elements[key]; }
      else general.push(msg);
    });
    if (general.length) showNotice("error", general[0]);
    else if (!firstField) showNotice("error", (data && data.message) || "Vui lòng kiểm tra lại thông tin.");
    if (firstField) firstField.focus();
  }

  function onSuccess(form, data) {
    if (form === forms.login) {
      redirecting = true; // giữ nút ở trạng thái loading đến khi chuyển trang
      showNotice("success", data.message || "Đăng nhập thành công! Đang chuyển hướng…");
      var target = data.redirectUrl || window.location.href;
      setTimeout(function () {
        var here = window.location.pathname + window.location.search;
        if (target === here) window.location.reload(); else window.location.href = target;
      }, 500);
      return;
    }
    // Gửi yêu cầu quên mật khẩu xong → báo đã gửi (cùng một câu dù email có tồn tại hay không)
    if (form === forms.forgot) {
      showNotice("success", data.message || "Nếu email đã đăng ký, chúng tôi đã gửi liên kết đặt lại mật khẩu.");
      showDevLink(data.devLink);
      return;
    }
    // Đặt mật khẩu mới xong → sang tab đăng nhập, điền sẵn email
    if (form === forms.reset) {
      form.reset();
      clearErrors(form);
      switchTab("login", false);
      forms.login.elements["Email"].value = data.email || "";
      forms.login.elements["Password"].value = "";
      showNotice("success", data.message || "Đặt lại mật khẩu thành công! Hãy đăng nhập bằng mật khẩu mới.");
      forms.login.elements[data.email ? "Password" : "Email"].focus();
      return;
    }
    // Đăng ký xong → sang tab đăng nhập, điền sẵn email
    var email = form.elements["Email"].value;
    form.reset();
    clearErrors(form);
    switchTab("login", false);
    forms.login.elements["Email"].value = email;
    forms.login.elements["Password"].value = "";
    showNotice("success", data.message || "Đăng ký thành công! Vui lòng đăng nhập.");
    forms.login.elements["Password"].focus();
  }

  function submit(form) {
    hideNotice();
    if (form === forms.forgot) hideDevLink();
    if (!validate(form)) return;
    setBusy(form, true);

    fetch(form.action, {
      method: "POST",
      body: new FormData(form),
      credentials: "same-origin",
      headers: { "X-Requested-With": "XMLHttpRequest", "Accept": "application/json" }
    })
      .then(function (res) {
        return res.text().then(function (text) {
          var data = null;
          try { data = JSON.parse(text); } catch (e) { /* không phải JSON */ }
          return { status: res.status, data: data };
        });
      })
      .then(function (r) {
        if (r.data && r.data.success) return onSuccess(form, r.data);
        if (r.data) return applyServerErrors(form, r.data);
        showNotice("error", r.status === 400
          ? "Phiên làm việc đã hết hạn. Vui lòng tải lại trang rồi thử lại."
          : "Có lỗi xảy ra, vui lòng thử lại sau.");
      })
      .catch(function () {
        showNotice("error", "Không thể kết nối máy chủ. Vui lòng kiểm tra mạng và thử lại.");
      })
      .then(function () { if (!redirecting) setBusy(form, false); });
  }

  /* ---------------- Sự kiện ---------------- */
  // Mở modal từ nút/link bất kỳ (giữ hành vi mặc định nếu Ctrl/Cmd/Shift/giữa chuột)
  document.addEventListener("click", function (e) {
    if (e.defaultPrevented || e.button !== 0 || e.metaKey || e.ctrlKey || e.shiftKey || e.altKey) return;

    var chk = e.target.closest ? e.target.closest("[data-auth-checkout]") : null;
    if (chk) {
      e.preventDefault();
      var targetUrl = chk.getAttribute("data-auth-checkout");
      if (forms.login && forms.login.elements["ReturnUrl"]) {
        forms.login.elements["ReturnUrl"].value = targetUrl;
      }
      open("login");
      showNotice("error", "Vui lòng đăng nhập hoặc đăng ký tài khoản để tiếp tục mua gói tập.");
      return;
    }
    var t = e.target.closest ? e.target.closest("[data-auth-open]") : null;
    if (!t || e.defaultPrevented || e.button !== 0 || e.metaKey || e.ctrlKey || e.shiftKey || e.altKey) return;
    e.preventDefault();
    var want = t.getAttribute("data-auth-open");
    open(want === "register" || want === "forgot" ? want : "login");
  });

  modal.addEventListener("click", function (e) {
    var el = e.target;
    if (el.closest("[data-am-close]")) { close(); return; }
    var tabEl = el.closest("[data-am-tab]");
    if (tabEl) {
      e.preventDefault(); // link "Quên mật khẩu?" có href làm dự phòng khi tắt JS
      var target = tabEl.dataset.amTab;
      // Sang quên mật khẩu: mang theo email đã gõ ở form đăng nhập
      var typed = forms.login.elements["Email"].value.trim();
      switchTab(target, true);
      if (target === "forgot" && typed && !forms.forgot.elements["Email"].value) {
        forms.forgot.elements["Email"].value = typed;
      }
      return;
    }

    var eye = el.closest("[data-am-toggle]");
    if (eye) {
      var input = eye.parentNode.querySelector("input");
      var show = input.type === "password";
      input.type = show ? "text" : "password";
      eye.setAttribute("aria-pressed", show ? "true" : "false");
      eye.setAttribute("aria-label", show ? "Ẩn mật khẩu" : "Hiện mật khẩu");
      input.focus();
      return;
    }

    var fill = el.closest("[data-am-fill]");
    if (fill) {
      forms.login.elements["Email"].value = fill.dataset.email;
      forms.login.elements["Password"].value = fill.dataset.pass;
      clearErrors(forms.login);
      forms.login.querySelector(".am-submit").focus();
    }
  });

  // Điều hướng tab bằng phím mũi tên (WAI-ARIA tabs)
  tabsBar.addEventListener("keydown", function (e) {
    if (e.key !== "ArrowLeft" && e.key !== "ArrowRight") return;
    if (TABS.indexOf(current) === -1) return;
    e.preventDefault();
    switchTab(current === "login" ? "register" : "login", false);
    modal.querySelector(".am-tab.is-on").focus();
  });

  Object.keys(forms).forEach(function (k) {
    var form = forms[k];
    form.addEventListener("submit", function (e) { e.preventDefault(); submit(form); });
    // Kiểm tra khi rời ô; sửa lỗi ngay khi gõ lại
    form.addEventListener("focusout", function (e) {
      var n = e.target && e.target.name;
      if (n && RULES[k][n] && e.target.value !== "") validateField(form, n);
    });
    form.addEventListener("input", function (e) {
      var n = e.target && e.target.name;
      if (!n) return;
      var f = fieldOf(form, n);
      if (f && f.classList.contains("is-invalid")) validateField(form, n);
      if (n === "Password" && form.elements["ConfirmPassword"] && form.elements["ConfirmPassword"].value) validateField(form, "ConfirmPassword");
    });
  });

  // Esc để đóng + giữ focus trong modal
  document.addEventListener("keydown", function (e) {
    if (!modal.classList.contains("open")) return;
    if (e.key === "Escape") { e.preventDefault(); close(); return; }
    if (e.key !== "Tab") return;
    var items = [].filter.call(
      box.querySelectorAll("a[href],button:not([disabled]),input:not([type=hidden]):not([disabled]),summary,[tabindex]:not([tabindex='-1'])"),
      function (el) { return el.offsetParent !== null; }
    );
    if (!items.length) return;
    var first = items[0], last = items[items.length - 1];
    if (e.shiftKey && (document.activeElement === first || !box.contains(document.activeElement))) { e.preventDefault(); last.focus(); }
    else if (!e.shiftKey && document.activeElement === last) { e.preventDefault(); first.focus(); }
  });

  window.AuthModal = { open: open, close: close };

  // ?auth=login | register | forgot | reset&token=... → mở sẵn
  try {
    var params = new URLSearchParams(window.location.search);
    var q = params.get("auth");
    if (q === "login" || q === "register" || q === "forgot" || q === "reset") {
      var token = params.get("token");
      if (q === "reset" && !token) q = "forgot";
      open(q);
      if (q === "reset") forms.reset.elements["Token"].value = token;
      if (q === "forgot" && params.get("reset") === "invalid") {
        showNotice("error", "Liên kết đặt lại mật khẩu không hợp lệ, đã hết hạn hoặc đã được dùng. Vui lòng gửi lại yêu cầu.");
      }
      // ?auth=login&returnUrl=/thanh-toan/... (server chuyển về khi chưa đăng nhập):
      // đăng nhập xong quay lại đúng trang đó. Chỉ nhận đường dẫn nội bộ.
      var ret = params.get("returnUrl");
      if (ret && ret.charAt(0) === "/" && ret.charAt(1) !== "/" && ret.charAt(1) !== "\\" && forms.login && forms.login.elements["ReturnUrl"]) {
        forms.login.elements["ReturnUrl"].value = ret;
        if (q === "login") {
          showNotice("error", ret.indexOf("/thanh-toan") === 0
            ? "Vui lòng đăng nhập hoặc đăng ký tài khoản để tiếp tục mua gói tập."
            : "Vui lòng đăng nhập để tiếp tục.");
        }
      }
      // Xoá auth/token/returnUrl khỏi thanh địa chỉ (không để token nằm lại trong lịch sử trình duyệt)
      if (window.history && history.replaceState) {
        var u = new URL(window.location.href);
        ["auth", "token", "reset", "returnUrl"].forEach(function (k) { u.searchParams.delete(k); });
        history.replaceState(null, "", u.pathname + u.search + u.hash);
      }
    }
  } catch (err) { /* trình duyệt cũ: bỏ qua */ }
})();
