/* Chatbot Pulse AI (góc trái). Gọi POST /AI/AskAssistant { message, history }.
   - Nhớ cuộc trò chuyện trong sessionStorage (mất khi đóng tab).
   - Render markdown rút gọn an toàn: **đậm**, *nghiêng*, danh sách, [link](/đường-dẫn). */
(function () {
  "use strict";

  var root = document.getElementById("pulseChat");
  if (!root) return;

  var fab = document.getElementById("pcFab");
  var panel = document.getElementById("pcPanel");
  var log = document.getElementById("pcLog");
  var chips = document.getElementById("pcChips");
  var form = document.getElementById("pcForm");
  var input = document.getElementById("pcInput");
  var sendBtn = document.getElementById("pcSend");
  var teaser = document.getElementById("pcTeaser");
  var endpoint = root.getAttribute("data-endpoint") || "/AI/AskAssistant";
  var userName = root.getAttribute("data-name") || "";

  var STORE = "pulseChat.v1";
  var TEASER_KEY = "pulseChat.teaser";
  var MAX_STORED = 30;
  var DEFAULT_CHIPS = ["Có những gói nào?", "Gói nào rẻ nhất?", "Danh sách HLV", "Giờ mở cửa?"];

  var messages = [];   // { role: 'user' | 'model', text }
  var busy = false;

  /* ---------------- storage (có thể bị chặn → bọc try/catch) ---------------- */
  function load() {
    try { var raw = sessionStorage.getItem(STORE); return raw ? JSON.parse(raw) : []; } catch (e) { return []; }
  }
  function save() {
    try { sessionStorage.setItem(STORE, JSON.stringify(messages.slice(-MAX_STORED))); } catch (e) { /* bỏ qua */ }
  }

  /* ---------------- render markdown rút gọn ---------------- */
  function esc(s) {
    return s.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;");
  }
  function inline(s) {
    s = esc(s);
    // [chữ](/đường-dẫn) hoặc [chữ](https://...) — chặn javascript: và //host
    s = s.replace(/\[([^\]]+)\]\(((?:\/(?!\/)|https?:\/\/)[^\s)]*)\)/g, function (m, t, u) {
      var ext = u.charAt(0) !== "/";
      return '<a href="' + u + '"' + (ext ? ' target="_blank" rel="noopener noreferrer"' : "") + ">" + t + "</a>";
    });
    s = s.replace(/\*\*([^*]+)\*\*/g, "<strong>$1</strong>");
    s = s.replace(/(^|[\s(>])\*([^*\s][^*]*?)\*(?=[\s).,!?:<]|$)/g, "$1<em>$2</em>");
    return s;
  }
  function render(text) {
    var lines = String(text).replace(/\r/g, "").split("\n");
    var html = "", list = null, para = [];
    function flushPara() { if (para.length) { html += "<p>" + para.join("<br>") + "</p>"; para = []; } }
    function flushList() { if (list) { html += "</" + list + ">"; list = null; } }
    lines.forEach(function (ln) {
      var ul = /^\s*[-*•]\s+(.*)$/.exec(ln);
      var ol = /^\s*\d+[.)]\s+(.*)$/.exec(ln);
      if (ul || ol) {
        flushPara();
        var kind = ul ? "ul" : "ol";
        if (list !== kind) { flushList(); html += "<" + kind + ">"; list = kind; }
        html += "<li>" + inline((ul || ol)[1]) + "</li>";
      } else if (!ln.trim()) {
        flushPara(); flushList();
      } else {
        flushList(); para.push(inline(ln));
      }
    });
    flushPara(); flushList();
    return html;
  }

  /* ---------------- DOM helpers ---------------- */
  function scrollDown() { log.scrollTop = log.scrollHeight; }

  function addMessage(role, text, opts) {
    var el = document.createElement("div");
    el.className = "pc-msg " + (role === "user" ? "user" : "bot") + (opts && opts.error ? " error" : "");
    if (role === "user") el.textContent = text; else el.innerHTML = render(text);
    log.appendChild(el);
    scrollDown();
    return el;
  }

  function showTyping() {
    var t = document.createElement("div");
    t.className = "pc-typing"; t.id = "pcTyping";
    t.setAttribute("aria-label", "Pulse AI đang trả lời");
    t.innerHTML = "<i></i><i></i><i></i>";
    log.appendChild(t); scrollDown();
  }
  function hideTyping() { var t = document.getElementById("pcTyping"); if (t) t.remove(); }

  function setChips(list) {
    chips.innerHTML = "";
    (list || []).slice(0, 4).forEach(function (txt) {
      var b = document.createElement("button");
      b.type = "button"; b.className = "pc-chip"; b.textContent = txt;
      b.addEventListener("click", function () { send(txt); });
      chips.appendChild(b);
    });
    scrollDown(); // thanh gợi ý làm khung tin nhắn thấp đi → cuộn lại xuống cuối
  }

  function welcome() {
    var hello = userName ? "Chào " + userName + "! " : "Chào bạn! ";
    var text = hello + "Mình là **Pulse AI** — trợ lý của Pulse Gym. Mình có thể giúp bạn về:\n" +
      "- **Gói tập** & bảng giá\n- **Huấn luyện viên (PT)**\n- **Khu vực tập**, giờ mở cửa\n- Cách **đăng ký & thanh toán**\n\nBạn muốn hỏi gì nào?";
    addMessage("model", text);
    setChips(DEFAULT_CHIPS);
  }

  function restore() {
    messages = load();
    log.innerHTML = "";
    if (!messages.length) { welcome(); return; }
    messages.forEach(function (m) { addMessage(m.role, m.text); });
    setChips(DEFAULT_CHIPS);
  }

  /* ---------------- mở / đóng ---------------- */
  function open() {
    root.classList.add("is-open");
    panel.setAttribute("aria-hidden", "false");
    fab.setAttribute("aria-expanded", "true");
    hideTeaser(true);
    setTimeout(function () { input.focus({ preventScroll: true }); scrollDown(); }, 120);
  }
  function close() {
    root.classList.remove("is-open");
    panel.setAttribute("aria-hidden", "true");
    fab.setAttribute("aria-expanded", "false");
    fab.focus({ preventScroll: true });
  }
  function toggle() { root.classList.contains("is-open") ? close() : open(); }

  /* ---------------- lời chào nhỏ ---------------- */
  function hideTeaser(remember) {
    teaser.hidden = true;
    if (remember) { try { sessionStorage.setItem(TEASER_KEY, "1"); } catch (e) { /* bỏ qua */ } }
  }
  function maybeTeaser() {
    var seen = false;
    try { seen = !!sessionStorage.getItem(TEASER_KEY); } catch (e) { /* bỏ qua */ }
    if (seen) return;
    setTimeout(function () { if (!root.classList.contains("is-open")) teaser.hidden = false; }, 4000);
    setTimeout(function () { if (!teaser.hidden) hideTeaser(true); }, 16000);
  }

  /* ---------------- gửi tin ---------------- */
  function setBusy(b) { busy = b; sendBtn.disabled = b; }

  function send(text) {
    text = (text || "").trim();
    if (!text || busy) return;

    var history = messages.slice(-10).map(function (m) { return { role: m.role, text: m.text }; });
    messages.push({ role: "user", text: text });
    addMessage("user", text);
    chips.innerHTML = "";
    input.value = ""; autosize();
    save();
    setBusy(true); showTyping();

    fetch(endpoint, {
      method: "POST",
      credentials: "same-origin",
      headers: { "Content-Type": "application/json", "Accept": "application/json" },
      body: JSON.stringify({ message: text, history: history })
    })
      .then(function (res) { if (!res.ok) throw new Error("HTTP " + res.status); return res.json(); })
      .then(function (data) {
        hideTyping();
        var reply = data && (data.reply || data.Reply);
        var ok = data && (data.success === undefined ? data.Success : data.success);
        if (reply) {
          messages.push({ role: "model", text: reply });
          addMessage("model", reply, { error: ok === false });
          setChips((data.suggestions || data.Suggestions) || DEFAULT_CHIPS);
        } else {
          addMessage("model", "Xin lỗi, mình chưa trả lời được lúc này. Bạn thử lại nhé!", { error: true });
          setChips(DEFAULT_CHIPS);
        }
        save();
      })
      .catch(function () {
        hideTyping();
        addMessage("model", "Không kết nối được máy chủ. Bạn kiểm tra mạng rồi thử lại nhé!", { error: true });
        setChips([text]); // cho phép bấm gửi lại đúng câu vừa hỏi
      })
      .then(function () { setBusy(false); if (root.classList.contains("is-open")) input.focus({ preventScroll: true }); });
  }

  /* ---------------- ô nhập ---------------- */
  function autosize() {
    input.style.height = "auto";
    input.style.height = Math.min(input.scrollHeight, 110) + "px";
  }

  form.addEventListener("submit", function (e) { e.preventDefault(); send(input.value); });
  input.addEventListener("input", autosize);
  input.addEventListener("keydown", function (e) {
    if (e.key === "Enter" && !e.shiftKey && !e.isComposing) { e.preventDefault(); send(input.value); }
  });

  fab.addEventListener("click", toggle);
  document.getElementById("pcClose").addEventListener("click", close);
  document.getElementById("pcTeaserX").addEventListener("click", function () { hideTeaser(true); });
  teaser.addEventListener("click", function (e) { if (!e.target.closest("#pcTeaserX")) open(); });

  document.getElementById("pcReset").addEventListener("click", function () {
    if (busy) return;
    messages = []; save(); restore(); input.focus({ preventScroll: true });
  });

  document.addEventListener("keydown", function (e) {
    if (e.key === "Escape" && root.classList.contains("is-open") && !document.querySelector(".am.open")) close();
  });

  // Mở cửa sổ chat từ nơi khác trên site: <a data-chat-open>
  document.addEventListener("click", function (e) {
    var t = e.target.closest ? e.target.closest("[data-chat-open]") : null;
    if (!t) return;
    e.preventDefault(); open();
  });

  window.PulseChat = { open: open, close: close, ask: function (q) { open(); send(q); } };

  restore();
  maybeTeaser();
})();
