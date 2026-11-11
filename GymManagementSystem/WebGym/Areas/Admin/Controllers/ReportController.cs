using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Admin;
using WebGym.Services.Helpers;

namespace WebGym.Areas.Admin.Controllers
{
    public class ReportController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Report
        public async Task<IActionResult> Index(int? year, int? month, string? sourceFilter, string? paymentMethod)
        {
            var report = await BuildConsolidatedReportAsync(year, month, sourceFilter, paymentMethod);
            return View(report);
        }

        // GET: /Admin/Report/ExportExcel
        public async Task<IActionResult> ExportExcel(int? year, int? month, string? sourceFilter, string? paymentMethod)
        {
            var report = await BuildConsolidatedReportAsync(year, month, sourceFilter, paymentMethod);
            var fileBytes = SimpleXlsxExporter.ExportRevenueReport(report);

            string periodSuffix = report.Month.HasValue && report.Month > 0
                ? $"{report.Year}_Thang{report.Month.Value:D2}"
                : $"{report.Year}";

            string fileName = $"BaoCaoDoanhThu_WebGym_{periodSuffix}.xlsx";

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        private async Task<RevenueReportDto> BuildConsolidatedReportAsync(int? year, int? month, string? sourceFilter, string? paymentMethod)
        {
            var selectedYear = year ?? DateTime.UtcNow.Year;
            var selectedMonth = month;
            var normalizedSource = string.IsNullOrWhiteSpace(sourceFilter) ? "All" : sourceFilter;

            var validOrderStatuses = new[] { "Hoàn thành", "Đã thanh toán" };

            // 1. Truy vấn Gói tập (Payments)
            var paymentsQuery = _context.Payments
                .Include(p => p.Package)
                .Include(p => p.Member).ThenInclude(m => m.User)
                .Where(p => p.Status == "Success" && p.PaymentDate.Year == selectedYear);

            // 2. Truy vấn Đơn hàng Shop / POS (Orders)
            var ordersQuery = _context.Orders
                .Include(o => o.Member).ThenInclude(m => m!.User)
                .Include(o => o.User)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .Where(o => validOrderStatuses.Contains(o.Status) && o.OrderDate.Year == selectedYear);

            // 3. Truy vấn Chi phí Bảo trì máy (MaintenanceLogs)
            var maintQuery = _context.MaintenanceLogs
                .Include(m => m.Equipment)
                .Where(m => m.MaintenanceDate.Year == selectedYear);

            if (selectedMonth.HasValue && selectedMonth > 0)
            {
                paymentsQuery = paymentsQuery.Where(p => p.PaymentDate.Month == selectedMonth.Value);
                ordersQuery = ordersQuery.Where(o => o.OrderDate.Month == selectedMonth.Value);
                maintQuery = maintQuery.Where(m => m.MaintenanceDate.Month == selectedMonth.Value);
            }

            if (!string.IsNullOrWhiteSpace(paymentMethod))
            {
                if (paymentMethod == "Tiền mặt")
                {
                    var cashNames = new[] { "Cash", "DirectCash", "Tiền mặt" };
                    paymentsQuery = paymentsQuery.Where(p => cashNames.Contains(p.PaymentMethod));
                    ordersQuery = ordersQuery.Where(o => cashNames.Contains(o.PaymentMethod));
                }
                else if (paymentMethod == "VNPay")
                {
                    var vnpayNames = new[] { "VNPay", "VnPay" };
                    paymentsQuery = paymentsQuery.Where(p => vnpayNames.Contains(p.PaymentMethod));
                    ordersQuery = ordersQuery.Where(o => vnpayNames.Contains(o.PaymentMethod));
                }
                else
                {
                    paymentsQuery = paymentsQuery.Where(p => p.PaymentMethod == paymentMethod);
                    ordersQuery = ordersQuery.Where(o => o.PaymentMethod == paymentMethod);
                }
            }

            var payments = await paymentsQuery.ToListAsync();
            var orders = await ordersQuery.ToListAsync();
            var maintLogs = await maintQuery.ToListAsync();

            decimal packageRevenue = payments.Sum(p => p.Amount);
            decimal shopRevenue = orders.Sum(o => o.TotalAmount);
            decimal maintenanceCost = maintLogs.Sum(m => m.Cost);

            // 4. Phân tích 12 tháng trong năm (Gói tập + Shop POS - Chi phí bảo trì)
            var yearPayments = await _context.Payments
                .Where(p => p.Status == "Success" && p.PaymentDate.Year == selectedYear)
                .Select(p => new { p.PaymentDate.Month, p.Amount })
                .ToListAsync();

            var yearOrders = await _context.Orders
                .Where(o => validOrderStatuses.Contains(o.Status) && o.OrderDate.Year == selectedYear)
                .Select(o => new { o.OrderDate.Month, o.TotalAmount })
                .ToListAsync();

            var yearMaint = await _context.MaintenanceLogs
                .Where(m => m.MaintenanceDate.Year == selectedYear)
                .Select(m => new { m.MaintenanceDate.Month, m.Cost })
                .ToListAsync();

            var monthlyBreakdown = new List<MonthlyRevenueItem>();
            for (int m = 1; m <= 12; m++)
            {
                decimal mPkg = yearPayments.Where(x => x.Month == m).Sum(x => x.Amount);
                decimal mShop = yearOrders.Where(x => x.Month == m).Sum(x => x.TotalAmount);
                decimal mMaint = yearMaint.Where(x => x.Month == m).Sum(x => x.Cost);

                monthlyBreakdown.Add(new MonthlyRevenueItem
                {
                    MonthLabel = $"Tháng {m}",
                    Revenue = mPkg + mShop,
                    PackageRevenue = mPkg,
                    ShopRevenue = mShop,
                    MaintenanceCost = mMaint
                });
            }

            // 5. Cơ cấu phương thức thanh toán (Hợp nhất cả Gói tập & Shop POS)
            string NormalizeMethod(string raw) => raw switch
            {
                "Cash" or "DirectCash" or "Tiền mặt" => "Tiền mặt",
                "VNPay" or "VnPay" => "VNPay",
                "BankTransfer" or "Chuyển khoản" => "Chuyển khoản",
                _ => raw
            };

            var methodBreakdown = new Dictionary<string, decimal>();
            foreach (var p in payments)
            {
                var key = NormalizeMethod(p.PaymentMethod);
                methodBreakdown[key] = methodBreakdown.GetValueOrDefault(key, 0) + p.Amount;
            }
            foreach (var o in orders)
            {
                var key = NormalizeMethod(o.PaymentMethod);
                methodBreakdown[key] = methodBreakdown.GetValueOrDefault(key, 0) + o.TotalAmount;
            }

            // 6. Gói tập bán chạy nhất
            var bestPkg = payments
                .GroupBy(p => p.Package.PackageName)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key ?? "Chưa có";

            // 7. Tổng hợp Sổ Giao Dịch Hợp Nhất
            var consolidatedItems = new List<ConsolidatedTransactionItem>();

            if (normalizedSource == "All" || normalizedSource == "Package")
            {
                foreach (var p in payments)
                {
                    consolidatedItems.Add(new ConsolidatedTransactionItem
                    {
                        Date = p.PaymentDate,
                        SourceType = "Gói tập",
                        ReferenceCode = p.TransactionCode,
                        CustomerOrTarget = p.Member.User.FullName,
                        Description = $"Đăng ký gói: {p.Package.PackageName}",
                        PaymentMethod = NormalizeMethod(p.PaymentMethod),
                        Amount = p.Amount,
                        IsExpense = false,
                        Status = "Thành công"
                    });
                }
            }

            if (normalizedSource == "All" || normalizedSource == "Shop")
            {
                foreach (var o in orders)
                {
                    string customerName = o.Member?.User?.FullName
                        ?? o.User?.FullName
                        ?? (o.ShippingAddress?.StartsWith("Khách vãng lai: ") == true
                            ? o.ShippingAddress.Replace("Khách vãng lai: ", "")
                            : "Khách lẻ tại quầy");

                    int itemCount = o.OrderDetails.Sum(od => od.Quantity);
                    string desc = o.OrderDetails.Any()
                        ? string.Join(", ", o.OrderDetails.Take(2).Select(od => $"{od.Product?.ProductName ?? "SP"} (x{od.Quantity})")) + (o.OrderDetails.Count > 2 ? "..." : "")
                        : $"Đơn hàng Shop ({itemCount} món)";

                    consolidatedItems.Add(new ConsolidatedTransactionItem
                    {
                        Date = o.OrderDate,
                        SourceType = "Shop POS",
                        ReferenceCode = $"#OD-{o.OrderId:D4}",
                        CustomerOrTarget = customerName,
                        Description = desc,
                        PaymentMethod = NormalizeMethod(o.PaymentMethod),
                        Amount = o.TotalAmount,
                        IsExpense = false,
                        Status = o.Status
                    });
                }
            }

            if ((normalizedSource == "All" || normalizedSource == "Maintenance") && string.IsNullOrWhiteSpace(paymentMethod))
            {
                foreach (var m in maintLogs)
                {
                    consolidatedItems.Add(new ConsolidatedTransactionItem
                    {
                        Date = m.MaintenanceDate,
                        SourceType = "Bảo trì",
                        ReferenceCode = $"#MT-{m.LogId:D4}",
                        CustomerOrTarget = m.Equipment?.EquipmentName ?? $"Thiết bị #{m.EquipmentId}",
                        Description = $"{m.Description} (Đơn vị: {m.PerformedBy})",
                        PaymentMethod = "Chi quỹ vận hành",
                        Amount = m.Cost,
                        IsExpense = true,
                        Status = m.Status
                    });
                }
            }

            consolidatedItems = consolidatedItems.OrderByDescending(i => i.Date).ToList();

            return new RevenueReportDto
            {
                Year = selectedYear,
                Month = selectedMonth,
                PaymentMethod = paymentMethod,
                SourceFilter = normalizedSource,
                PackageRevenue = packageRevenue,
                ShopRevenue = shopRevenue,
                MaintenanceCost = maintenanceCost,
                TotalRevenue = packageRevenue + shopRevenue,
                TotalTransactions = payments.Count + orders.Count,
                PackageTransactionsCount = payments.Count,
                ShopOrdersCount = orders.Count,
                MaintenanceLogsCount = maintLogs.Count,
                BestSellingPackage = bestPkg,
                MonthlyBreakdown = monthlyBreakdown,
                MethodBreakdown = methodBreakdown,
                ConsolidatedItems = consolidatedItems
            };
        }
    }
}
