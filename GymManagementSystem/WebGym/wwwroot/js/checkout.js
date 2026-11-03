(function () {
  var sum = document.getElementById("coSum");
  if (!sum) return;
  var monthly = +sum.dataset.monthly;
  var fmt = function (n) { return n.toLocaleString("vi-VN") + "đ"; };
  var $ = function (id) { return document.getElementById(id); };

  function update() {
    var year = document.getElementById("cy-year").checked;
    var months = year ? 12 : 1;
    var base = monthly * months;
    var total = year ? Math.round(monthly * 0.8) * 12 : base;
    $("coMonths").textContent = months;
    $("coBase").textContent = fmt(base);
    $("coDiscAmt").textContent = fmt(base - total);
    $("coDisc").hidden = !year;
    $("coTotal").textContent = fmt(total);
    $("coBtn").textContent = fmt(total);
    $("coNote").textContent = year ? "Thanh toán một lần cho 12 tháng." : "Thanh toán theo từng tháng.";
  }
  document.querySelectorAll('input[name="Cycle"]').forEach(function (r) { r.addEventListener("change", update); });
})();
