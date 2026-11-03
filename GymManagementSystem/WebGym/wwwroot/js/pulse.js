(function () {
  // ===== Lịch tập: sửa dữ liệu ở đây (hoặc đổ từ Controller sau) =====
  var days = [
    { t: "T2", n: "Thứ 2", c: [["06:00", "Power HIIT", "Anh Tuấn", 4], ["17:30", "Boxing Core", "Hoàng Nam", 8], ["19:00", "Strength 101", "Minh Khôi", 6]] },
    { t: "T3", n: "Thứ 3", c: [["06:30", "Yoga Flow", "Thu Hà", 10], ["18:00", "Gym Coaching", "Minh Khôi", 5], ["19:30", "HIIT Burn", "Anh Tuấn", 7]] },
    { t: "T4", n: "Thứ 4", c: [["06:00", "Strength 101", "Minh Khôi", 6], ["17:30", "Boxing Basic", "Hoàng Nam", 9], ["19:00", "Mobility", "Thu Hà", 12]] },
    { t: "T5", n: "Thứ 5", c: [["06:00", "Power HIIT", "Anh Tuấn", 3], ["18:00", "Boxing Core", "Hoàng Nam", 8], ["19:30", "Yoga Flow", "Thu Hà", 10]] },
    { t: "T6", n: "Thứ 6", c: [["06:30", "Gym Coaching", "Minh Khôi", 5], ["17:30", "HIIT Burn", "Anh Tuấn", 6], ["19:00", "Boxing Sparring", "Hoàng Nam", 4]] },
    { t: "T7", n: "Thứ bảy", c: [["08:00", "Full Body", "Minh Khôi", 12], ["09:30", "Yoga Flow", "Thu Hà", 15], ["16:00", "Boxing Core", "Hoàng Nam", 10]] }
  ];
  var tabs = document.getElementById("tabs"), list = document.getElementById("classes");

  function show(i) {
    Array.prototype.forEach.call(tabs.children, function (b, k) { b.classList.toggle("on", k === i); });
    list.innerHTML = days[i].c.map(function (x, k) {
      return '<div class="class"><span class="idx">0' + (k + 1) + '</span><span class="time">' + x[0] +
        '</span><div><h4>' + x[1] + '</h4><p>Với HLV ' + x[2] + '</p></div><span class="slots">' +
        String(x[3]).padStart(2, "0") + ' chỗ</span><a href="#" class="arrow-circle">→</a></div>';
    }).join("");
  }
  days.forEach(function (d, i) {
    var b = document.createElement("button");
    b.innerHTML = d.t + "<small>" + d.n + "</small>";
    b.onclick = function () { show(i); };
    tabs.appendChild(b);
  });
  show(0);

  // ===== Bảng giá: tháng / năm (-20%) =====
  var fmt = function (n) { return n.toLocaleString("vi-VN"); };
  document.querySelectorAll("#billing button").forEach(function (btn) {
    btn.addEventListener("click", function () {
      document.querySelectorAll("#billing button").forEach(function (b) { b.classList.toggle("on", b === btn); });
      var year = btn.dataset.mode === "year";
      document.querySelectorAll("[data-plan-link]").forEach(function (a) {
        if (!a.dataset.base) a.dataset.base = a.getAttribute("href");
        a.setAttribute("href", year ? a.dataset.base + "?cycle=year" : a.dataset.base);
      });
      document.querySelectorAll("[data-price]").forEach(function (el) {
        var p = +el.dataset.price;
        el.textContent = fmt(year ? Math.round(p * 0.8) : p);
      });
    });
  });
})();

// ===== Video giới thiệu =====
(function () {
  var btn = document.getElementById("playIntro"),
      modal = document.getElementById("videoModal"),
      video = document.getElementById("introVideo"),
      box = modal.querySelector(".video-box");
  if (!btn) return;

  function open() {
    modal.classList.add("open");
    document.body.style.overflow = "hidden";
    var p = video.play();
    if (p && p.catch) p.catch(function () {});
    modal.querySelector(".video-close").focus();
  }
  function close() {
    modal.classList.remove("open");
    document.body.style.overflow = "";
    video.pause();
    video.currentTime = 0;
    btn.focus();
  }
  btn.addEventListener("click", open);
  modal.querySelectorAll("[data-close]").forEach(function (el) { el.addEventListener("click", close); });
  document.addEventListener("keydown", function (e) {
    if (e.key === "Escape" && modal.classList.contains("open")) close();
  });
  video.querySelector("source").addEventListener("error", function () { box.classList.add("failed"); });
})();
