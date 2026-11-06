// ===== Công cụ BMI / TDEE =====
(function () {
  var form = document.getElementById("bmiForm");
  if (!form) return;

  var cfg = { plans: [], areas: {} };
  try { cfg = JSON.parse(document.getElementById("bmiConfig").textContent); } catch (e) {}

  var $ = function (id) { return document.getElementById(id); };
  var esc = function (s) {
    return String(s == null ? "" : s).replace(/[&<>"']/g, function (c) {
      return { "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" }[c];
    });
  };
  var num = function (n, d) {
    return n.toLocaleString("vi-VN", { minimumFractionDigits: d || 0, maximumFractionDigits: d || 0 });
  };
  var round10 = function (n) { return Math.round(n / 10) * 10; };

  // Phân loại BMI theo chuẩn Châu Á - Thái Bình Dương
  var CATS = {
    under:  { t: "Thiếu cân",     c: "#6f8cff" },
    normal: { t: "Bình thường",   c: "#c8ff2e" },
    over:   { t: "Thừa cân",      c: "#ffcf4d" },
    ob1:    { t: "Béo phì độ I",  c: "#ff9a3d" },
    ob2:    { t: "Béo phì độ II", c: "#ff5e5e" }
  };
  function category(b) {
    if (b < 18.5) return "under";
    if (b < 23) return "normal";
    if (b < 25) return "over";
    if (b < 30) return "ob1";
    return "ob2";
  }

  var STATUS = {
    under:  "Cân nặng của bạn đang thấp hơn mức khuyến nghị. Cơ thể có thể thiếu năng lượng và khối cơ, nên cần ăn đủ và tập sức mạnh để tăng cân lành mạnh.",
    normal: "Chỉ số của bạn nằm trong mức khỏe mạnh. Bạn có thể tập trung vào sức mạnh, thể lực và hình thể.",
    over:   "Bạn đang ở mức thừa cân nhẹ theo chuẩn Châu Á. Điều chỉnh ăn uống và tập luyện đều đặn sẽ giúp đưa chỉ số về mức lý tưởng.",
    ob1:    "Bạn đang ở mức béo phì độ I. Giảm mỡ từ từ kết hợp tập luyện đều đặn sẽ giúp cải thiện sức khỏe rõ rệt.",
    ob2:    "Bạn đang ở mức béo phì độ II. Nên bắt đầu thật nhẹ nhàng, có người hướng dẫn và thay đổi ăn uống dần dần để giảm nguy cơ cho sức khỏe."
  };

  var TIPS = {
    under: [
      "Ưu tiên tập tạ 3 buổi mỗi tuần với các bài nhiều khớp như squat, đẩy ngực, kéo xà; hạn chế cardio quá nhiều.",
      "Ăn đủ 3 bữa chính và 1 đến 2 bữa phụ, tăng lượng ăn từ từ thay vì ăn quá nhiều trong một lúc.",
      "Ngủ 7 đến 9 tiếng mỗi đêm vì cơ bắp phát triển trong lúc nghỉ ngơi."
    ],
    normal: [
      "Duy trì 3 đến 4 buổi tập mỗi tuần, kết hợp tạ và cardio để giữ thể lực cân đối.",
      "Tăng mức tạ dần theo từng giai đoạn để cơ thể tiếp tục tiến bộ.",
      "Uống đủ nước và ngủ đủ giấc để phục hồi tốt."
    ],
    over: [
      "Kết hợp cardio cường độ vừa 3 đến 4 buổi mỗi tuần với 2 đến 3 buổi tạ để giảm mỡ mà vẫn giữ cơ.",
      "Giảm đồ uống có đường và đồ chiên rán; giảm khoảng 0,25 đến 0,5 kg mỗi tuần là tốc độ bền vững.",
      "Tăng dần cường độ, tránh tập quá sức ngay từ đầu để hạn chế chấn thương."
    ],
    ob2: [
      "Bắt đầu với các bộ môn ít tác động lên khớp như đạp xe, máy elliptical và đi bộ.",
      "Nên có huấn luyện viên theo sát vài tuần đầu để tập đúng kỹ thuật và an toàn.",
      "Giảm dần lượng ăn và đồ uống có đường, không nhịn ăn cực đoan."
    ]
  };
  TIPS.ob1 = TIPS.over;

  var AREA_NAMES = { weights: "Khu tạ tự do", cardio: "Khu cardio", boxing: "Phòng boxing", yoga: "Studio yoga" };
  function areasFor(k, goal) {
    if (k === "under") return ["weights", "yoga"];
    if (k === "ob2") return ["cardio", "yoga"];
    if (k === "over" || k === "ob1") return ["cardio", "weights"];
    if (goal === "gain") return ["weights", "boxing"];
    if (goal === "lose") return ["cardio", "boxing"];
    return ["weights", "cardio", "yoga"];
  }

  function recommendPlan(k, act) {
    var plans = cfg.plans || [];
    if (!plans.length) return null;
    var pts = 0, why = [];
    if (k === "under" || k === "ob1" || k === "ob2") { pts++; why.push("chỉ số BMI của bạn cần được hướng dẫn tập luyện và ăn uống cẩn thận"); }
    if (act <= 1.375) { pts++; why.push("mức vận động hiện tại còn ít nên cần người đồng hành để xây thói quen"); }
    if (k === "ob2") pts++;
    var n = plans.length, i = pts === 0 ? 0 : pts === 1 ? Math.floor((n - 1) / 2) : n - 1;
    var reason = pts === 0
      ? "Bạn đã có nền tảng tốt, một gói cơ bản là đủ để bắt đầu và tự tập."
      : "Gợi ý này dựa trên việc " + why.join(" và ") + ".";
    return { plan: plans[i], reason: reason };
  }

  function show(r) {
    $("bmiEmpty").hidden = true;
    $("bmiOut").hidden = false;

    $("bmiVal").textContent = num(r.bmi, 1);
    var cat = $("bmiCat");
    cat.textContent = CATS[r.k].t;
    cat.style.background = CATS[r.k].c;
    $("bmiMark").style.left = Math.max(1, Math.min(99, (r.bmi - 14) / 22 * 100)) + "%";

    $("bmiBmr").textContent = num(Math.round(r.bmr));
    $("bmiTdee").textContent = num(Math.round(r.tdee));
    $("bmiTarget").textContent = num(r.target);
    $("bmiProtein").textContent = r.protein;

    $("bmiStatus").textContent = STATUS[r.k];
    var range = $("bmiRange"); // bỏ qua nếu giao diện không có ô này, tránh làm dừng cả phần kết quả
    if (range) {
      range.textContent = "Cân nặng hợp lý cho chiều cao của bạn là khoảng " +
        num(r.low, 1) + " đến " + num(r.high, 1) + " kg.";
    }

    var note = $("bmiNote");
    note.hidden = !r.note;
    note.textContent = r.note || "";

    $("bmiTips").innerHTML = r.tips.map(function (t) { return "<li>" + esc(t) + "</li>"; }).join("");

    $("bmiAreas").innerHTML = r.areas.map(function (a) {
      var url = (cfg.areas && cfg.areas[a]) || "#";
      return '<li><a href="' + esc(url) + '">' + AREA_NAMES[a] + "</a></li>";
    }).join("");

    var box = $("bmiPlan");
    if (r.rec) {
      var p = r.rec.plan;
      box.hidden = false;
      box.innerHTML =
        "<div><small>Gói tập phù hợp với bạn</small><h4>" + esc(p.name) + "</h4>" +
        (p.desc ? "<p>" + esc(p.desc) + "</p>" : "") +
        '<p class="bmi-why">' + esc(r.rec.reason) + "</p></div>" +
        '<div class="bmi-plan-cta"><strong>' + esc(p.price) + "</strong><span>/ " + esc(p.duration) + "</span>" +
        '<a class="btn" href="' + esc(p.url) + '">Đăng ký gói này <b>→</b></a>' +
        '<a class="link-ul" href="#bang-gia">So sánh các gói</a></div>';
    } else {
      box.hidden = true;
    }

    if (window.innerWidth < 1000) {
      $("bmiResult").scrollIntoView({ behavior: "smooth", block: "start" });
    }
  }

  form.addEventListener("submit", function (e) {
    e.preventDefault();
    var err = $("bmiErr");
    var male = form.elements.gender.value === "m";
    var goal = form.elements.goal.value;
    var age = +$("bmiAge").value, h = +$("bmiH").value, w = +$("bmiW").value, act = +$("bmiAct").value;

    var msg = "";
    if (!(age >= 18 && age <= 80)) msg = "Công cụ dành cho người từ 18 đến 80 tuổi.";
    else if (!(h >= 120 && h <= 220)) msg = "Chiều cao hợp lệ từ 120 đến 220 cm.";
    else if (!(w >= 30 && w <= 250)) msg = "Cân nặng hợp lệ từ 30 đến 250 kg.";
    err.textContent = msg;
    if (msg) return;

    var hm = h / 100;
    var bmi = w / (hm * hm);
    var k = category(bmi);
    // Mifflin-St Jeor
    var bmr = 10 * w + 6.25 * h - 5 * age + (male ? 5 : -161);
    var tdee = bmr * act;

    var note = "";
    if (k === "under" && goal === "lose") {
      goal = "gain";
      note = "Với chỉ số hiện tại, bạn không nên giảm thêm cân. Chúng tôi đã đổi mục tiêu sang tăng cơ để phù hợp hơn.";
    } else if ((k === "ob1" || k === "ob2") && goal === "gain") {
      goal = "lose";
      note = "Với chỉ số hiện tại, nên ưu tiên giảm mỡ trước và vẫn tập tạ để giữ cơ. Chúng tôi đã đổi mục tiêu sang giảm mỡ.";
    }

    var target, protein;
    if (goal === "lose") {
      target = round10(Math.max(tdee * 0.85, bmr));
      protein = Math.round(w * 1.6) + " – " + Math.round(w * 2.0);
    } else if (goal === "gain") {
      target = round10(tdee + 300);
      protein = Math.round(w * 1.6) + " – " + Math.round(w * 2.0);
    } else {
      target = round10(tdee);
      protein = Math.round(w * 1.2) + " – " + Math.round(w * 1.6);
    }

    var tips = TIPS[k].slice();
    if (goal === "lose") tips.push("Mục tiêu khoảng " + num(target) + " kcal mỗi ngày, thấp hơn mức duy trì khoảng 15%.");
    else if (goal === "gain") tips.push("Mục tiêu khoảng " + num(target) + " kcal mỗi ngày, cao hơn mức duy trì khoảng 300 kcal, kèm đủ protein.");
    else tips.push("Giữ khoảng " + num(target) + " kcal mỗi ngày để duy trì cân nặng hiện tại.");
    if (bmi < 16 || bmi >= 35 || age >= 60) {
      tips.push("Với tình trạng của bạn, nên tham khảo ý kiến bác sĩ trước khi bắt đầu tập luyện cường độ cao.");
    }

    show({
      bmi: bmi, k: k, bmr: bmr, tdee: tdee, target: target, protein: protein,
      low: 18.5 * hm * hm, high: 22.9 * hm * hm,
      note: note, tips: tips, areas: areasFor(k, goal), rec: recommendPlan(k, act)
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

// ===== Scroll Reveal & Dynamic Animations =====
(function () {
  "use strict";

  // 1. Thanh tiến độ cuộn trang neon trên cùng
  var bar = document.createElement("div");
  bar.className = "scroll-progress";
  bar.setAttribute("aria-hidden", "true");
  document.body.appendChild(bar);

  var nav = document.querySelector(".nav");
  function onScroll() {
    var winScroll = window.scrollY || document.documentElement.scrollTop;
    var height = document.documentElement.scrollHeight - document.documentElement.clientHeight;
    var pct = height > 0 ? (winScroll / height) * 100 : 0;
    bar.style.width = pct + "%";
    if (nav) nav.classList.toggle("is-scrolled", winScroll > 40);
  }
  window.addEventListener("scroll", onScroll, { passive: true });
  onScroll();

  // 2. Gán tự động các class hiệu ứng vào các khối chính
  document.querySelectorAll(".section .head, .pricing > .eyebrow, .pricing > .display, .cta > div, .footer-grid > div").forEach(function (el) {
    el.classList.add("reveal");
  });

  // Cơ sở vật chất: so le từng thẻ
  document.querySelectorAll(".programs .program").forEach(function (el, idx) {
    el.classList.add("reveal", "delay-" + ((idx % 4) + 1));
  });

  // BMI Tool: trượt mượt từ 2 bên
  var bmiForm = document.querySelector(".bmi-card");
  var bmiPreview = document.querySelector(".bmi-preview");
  if (bmiForm) bmiForm.classList.add("reveal-left");
  if (bmiPreview) bmiPreview.classList.add("reveal-right");

  // Huấn luyện viên: so le
  document.querySelectorAll(".trainers .trainer-card").forEach(function (el, idx) {
    el.classList.add("reveal", "delay-" + ((idx % 3) + 1));
  });

  // Bảng giá: so le dạng scale nhẹ
  document.querySelectorAll(".plans .plan").forEach(function (el, idx) {
    el.classList.add("reveal-scale", "delay-" + ((idx % 4) + 1));
  });

  // Stats items
  document.querySelectorAll(".stats li").forEach(function (el, idx) {
    el.classList.add("reveal", "delay-" + ((idx % 4) + 1));
  });

  // Hero elements
  var heroText = document.querySelector(".hero-text");
  var heroMedia = document.querySelector(".hero-media");
  if (heroText) heroText.classList.add("reveal");
  if (heroMedia) heroMedia.classList.add("reveal-scale", "delay-2");

  // 3. IntersectionObserver kích hoạt hiệu ứng khi cuộn tới
  if ("IntersectionObserver" in window) {
    var observer = new IntersectionObserver(function (entries, obs) {
      entries.forEach(function (entry) {
        if (entry.isIntersecting) {
          entry.target.classList.add("is-revealed");
          obs.unobserve(entry.target);
        }
      });
    }, {
      rootMargin: "0px 0px -50px 0px",
      threshold: 0.12
    });

    document.querySelectorAll(".reveal, .reveal-fade, .reveal-left, .reveal-right, .reveal-scale").forEach(function (el) {
      observer.observe(el);
    });
  } else {
    document.querySelectorAll(".reveal, .reveal-fade, .reveal-left, .reveal-right, .reveal-scale").forEach(function (el) {
      el.classList.add("is-revealed");
    });
  }

  // 4. Hiệu ứng đếm số (Counting Numbers Animation) khi lướt tới
  function animateNumber(el, target, suffix, isDecimal) {
    var duration = 1800;
    var start = 0;
    var startTime = null;

    function step(timestamp) {
      if (!startTime) startTime = timestamp;
      var progress = Math.min((timestamp - startTime) / duration, 1);
      var ease = 1 - Math.pow(1 - progress, 3);
      var current = start + (target - start) * ease;
      el.innerHTML = (isDecimal ? current.toFixed(1) : Math.floor(current)) + suffix;
      if (progress < 1) {
        window.requestAnimationFrame(step);
      } else {
        el.innerHTML = (isDecimal ? target.toFixed(1) : target) + suffix;
      }
    }
    window.requestAnimationFrame(step);
  }

  var statsContainer = document.querySelector(".stats");
  if (statsContainer && "IntersectionObserver" in window) {
    var statsObserver = new IntersectionObserver(function (entries, obs) {
      entries.forEach(function (entry) {
        if (entry.isIntersecting) {
          var items = statsContainer.querySelectorAll("strong");
          if (items[0]) animateNumber(items[0], 2.5, "K<sup>+</sup>", true);
          if (items[1]) animateNumber(items[1], 25, "<sup>+</sup>", false);
          if (items[2]) animateNumber(items[2], 12, "", false);
          obs.unobserve(statsContainer);
        }
      });
    }, { threshold: 0.25 });
    statsObserver.observe(statsContainer);
  }

  var chipSat = document.querySelector(".chip-sat strong");
  if (chipSat && "IntersectionObserver" in window) {
    var chipObserver = new IntersectionObserver(function (entries, obs) {
      entries.forEach(function (entry) {
        if (entry.isIntersecting) {
          animateNumber(chipSat, 98, "%", false);
          obs.unobserve(chipSat);
        }
      });
    }, { threshold: 0.3 });
    chipObserver.observe(chipSat);
  }
})();
