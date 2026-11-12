using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Admin;
using WebGym.Models.Entities;

namespace WebGym.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderManagementController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public OrderManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/OrderManagement
        public async Task<IActionResult> Index(
            [FromQuery(Name = "statusFilter")] string? statusFilter,
            [FromQuery(Name = "paymentFilter")] string? paymentFilter,
            [FromQuery(Name = "fromDate")] DateTime? fromDate,
            [FromQuery(Name = "toDate")] DateTime? toDate,
            [FromQuery(Name = "keyword")] string? keyword)
        {
            var query = _context.Orders
                .Include(o => o.Member).ThenInclude(m => m!.User)
                .Include(o => o.User)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                query = query.Where(o => o.Status == statusFilter);
            }

            if (!string.IsNullOrWhiteSpace(paymentFilter))
            {
                query = query.Where(o => o.PaymentMethod == paymentFilter);
            }

            if (fromDate.HasValue)
            {
                var start = fromDate.Value.Date;
                query = query.Where(o => o.OrderDate >= start);
            }

            if (toDate.HasValue)
            {
                var end = toDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(o => o.OrderDate <= end);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword.Trim().ToLower();
                query = query.Where(o => o.OrderId.ToString().Contains(kw) ||
                                         (o.PhoneNumber != null && o.PhoneNumber.Contains(kw)) ||
                                         (o.Member != null && o.Member.User.FullName.ToLower().Contains(kw)) ||
                                         (o.User != null && o.User.FullName.ToLower().Contains(kw)) ||
                                         (o.Notes != null && o.Notes.ToLower().Contains(kw)));
            }

            var orders = await query.OrderByDescending(o => o.OrderDate).ToListAsync();
            var allOrders = await _context.Orders.AsNoTracking().ToListAsync();

            var viewModel = new OrderFilterViewModel
            {
                Status = statusFilter,
                PaymentMethod = paymentFilter,
                FromDate = fromDate,
                ToDate = toDate,
                SearchKeyword = keyword,
                Orders = orders,
                TotalOrders = allOrders.Count,
                TotalRevenue = allOrders.Where(o => o.Status == "Hoàn thành" || o.Status == "Đã thanh toán").Sum(o => o.TotalAmount),
                CompletedOrders = allOrders.Count(o => o.Status == "Hoàn thành" || o.Status == "Đã thanh toán"),
                PendingOrders = allOrders.Count(o => o.Status == "Chờ xác nhận" || o.Status == "Đang giao"),
                CancelledOrders = allOrders.Count(o => o.Status == "Đã hủy")
            };

            return View(viewModel);
        }

        // GET: /Admin/OrderManagement/POS
        public async Task<IActionResult> POS()
        {
            var categories = await _context.ProductCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .OrderBy(p => p.ProductName)
                .ToListAsync();

            var members = await _context.Members
                .Include(m => m.User)
                .OrderBy(m => m.User.FullName)
                .ToListAsync();

            var viewModel = new PosScreenViewModel
            {
                Categories = categories,
                Products = products,
                Members = members
            };

            return View(viewModel);
        }

        // POST: /Admin/OrderManagement/CreatePosOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePosOrder([FromBody] PosCheckoutDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.CartItemsJson))
            {
                return Json(new { success = false, message = "Giỏ hàng rỗng! Vui lòng chọn sản phẩm." });
            }

            List<PosCartItemDto>? cartItems;
            try
            {
                cartItems = JsonSerializer.Deserialize<List<PosCartItemDto>>(dto.CartItemsJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch
            {
                return Json(new { success = false, message = "Dữ liệu giỏ hàng không hợp lệ." });
            }

            if (cartItems == null || !cartItems.Any())
            {
                return Json(new { success = false, message = "Vui lòng chọn ít nhất 1 sản phẩm vào giỏ hàng." });
            }

            // Kiểm tra tồn kho của từng mặt hàng
            var productIds = cartItems.Select(c => c.ProductId).ToList();
            var productsInDb = await _context.Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToDictionaryAsync(p => p.ProductId);

            foreach (var item in cartItems)
            {
                if (!productsInDb.TryGetValue(item.ProductId, out var prod))
                {
                    return Json(new { success = false, message = $"Sản phẩm '{item.ProductName}' không còn tồn tại trên hệ thống." });
                }

                if (prod.StockQuantity < item.Quantity)
                {
                    return Json(new { success = false, message = $"Sản phẩm '{prod.ProductName}' chỉ còn {prod.StockQuantity} cái trong kho, không đủ số lượng yêu cầu ({item.Quantity})." });
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                decimal totalAmount = 0;
                var orderDetails = new List<OrderDetail>();

                foreach (var item in cartItems)
                {
                    var prod = productsInDb[item.ProductId];
                    decimal lineTotal = prod.Price * item.Quantity;
                    totalAmount += lineTotal;

                    // Trừ tồn kho
                    prod.StockQuantity -= item.Quantity;

                    orderDetails.Add(new OrderDetail
                    {
                        ProductId = prod.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = prod.Price,
                        TotalPrice = lineTotal
                    });
                }

                int? memberId = null;
                int? userId = null;
                string? customerPhone = dto.GuestPhone;
                string shippingAddress = "Bán lẻ tại quầy POS";

                if (!dto.IsWalkInGuest && dto.MemberId.HasValue && dto.MemberId.Value > 0)
                {
                    memberId = dto.MemberId.Value;
                    var member = await _context.Members.Include(m => m.User).FirstOrDefaultAsync(m => m.MemberId == memberId);
                    if (member != null)
                    {
                        userId = member.UserId;
                        customerPhone = member.User.PhoneNumber;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(dto.GuestName))
                    {
                        shippingAddress = $"Khách vãng lai: {dto.GuestName.Trim()}";
                    }
                }

                var order = new Order
                {
                    MemberId = memberId,
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = totalAmount,
                    Status = "Hoàn thành",
                    PaymentMethod = string.IsNullOrWhiteSpace(dto.PaymentMethod) ? "Tiền mặt" : dto.PaymentMethod,
                    ShippingAddress = shippingAddress,
                    PhoneNumber = customerPhone,
                    Notes = string.IsNullOrWhiteSpace(dto.Notes) ? "Đơn hàng thanh toán tại quầy POS" : dto.Notes.Trim(),
                    OrderDetails = orderDetails
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Json(new
                {
                    success = true,
                    orderId = order.OrderId,
                    totalAmount = order.TotalAmount,
                    message = $"Tạo đơn hàng #{order.OrderId} thành công!"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = "Đã xảy ra lỗi khi tạo đơn hàng: " + ex.Message });
            }
        }

        // GET: /Admin/OrderManagement/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Member).ThenInclude(m => m!.User)
                .Include(o => o.User)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product).ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng yêu cầu.";
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }

        // GET: /Admin/OrderManagement/PrintReceipt/5
        public async Task<IActionResult> PrintReceipt(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Member).ThenInclude(m => m!.User)
                .Include(o => o.User)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy hóa đơn cần in.";
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }

        // POST: /Admin/OrderManagement/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string newStatus)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction(nameof(Index));
            }

            order.Status = newStatus;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã cập nhật trạng thái đơn hàng #{order.OrderId} thành '{newStatus}'.";

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: /Admin/OrderManagement/CancelOrder/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction(nameof(Index));
            }

            if (order.Status == "Đã hủy")
            {
                TempData["ErrorMessage"] = "Đơn hàng này đã được hủy trước đó.";
                return RedirectToAction(nameof(Index));
            }

            // Hoàn lại số lượng tồn kho nếu đơn đã trừ
            foreach (var detail in order.OrderDetails)
            {
                if (detail.Product != null)
                {
                    detail.Product.StockQuantity += detail.Quantity;
                }
            }

            order.Status = "Đã hủy";
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Đã hủy đơn hàng #{order.OrderId} và hoàn lại tồn kho thành công!";

            return RedirectToAction(nameof(Index));
        }
    }
}

