using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Admin;
using WebGym.Models.Entities;

namespace WebGym.Areas.Admin.Controllers
{
    public class EquipmentManagementController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EquipmentManagementController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: /Admin/EquipmentManagement
        public async Task<IActionResult> Index(
            string? searchKeyword, 
            int? roomId, 
            [FromQuery(Name = "areaFilter")] string? areaFilter, 
            string? brand, 
            string? status)
        {
            var query = _context.Equipments
                .Include(e => e.GymRoom)
                .Include(e => e.MaintenanceLogs)
                .AsQueryable();

            // Tìm kiếm từ khóa
            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                var kw = searchKeyword.Trim().ToLower();
                query = query.Where(e => e.EquipmentName.ToLower().Contains(kw) ||
                                         (e.SerialNumber != null && e.SerialNumber.ToLower().Contains(kw)) ||
                                         e.Brand.ToLower().Contains(kw));
            }

            // Lọc theo phòng
            if (roomId.HasValue && roomId.Value > 0)
            {
                query = query.Where(e => e.RoomId == roomId.Value);
            }

            // Lọc theo khu vực
            if (!string.IsNullOrWhiteSpace(areaFilter))
            {
                query = query.Where(e => e.Area == areaFilter);
            }

            // Lọc theo hãng sản xuất
            if (!string.IsNullOrWhiteSpace(brand))
            {
                query = query.Where(e => e.Brand == brand);
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(e => e.Status == status);
            }

            var equipments = await query.OrderByDescending(e => e.EquipmentId).ToListAsync();

            // Thống kê nhanh
            ViewBag.TotalCount = await _context.Equipments.CountAsync();
            ViewBag.GoodCount = await _context.Equipments.CountAsync(e => e.Status == "Hoạt động tốt");
            ViewBag.WarningCount = await _context.Equipments.CountAsync(e => e.Status == "Cần bảo trì");
            ViewBag.BrokenCount = await _context.Equipments.CountAsync(e => e.Status == "Đang hỏng");

            // Danh sách lựa chọn cho bộ lọc
            var rooms = await _context.GymRooms.OrderBy(r => r.RoomName).ToListAsync();
            ViewBag.RoomsList = new SelectList(rooms, "RoomId", "RoomName", roomId);

            var areaList = await _context.Equipments.Select(e => e.Area).Distinct().Where(a => a != null).ToListAsync();
            ViewBag.AreaList = new SelectList(areaList, areaFilter);

            var brandList = await _context.Equipments.Select(e => e.Brand).Distinct().Where(b => b != null).ToListAsync();
            ViewBag.BrandList = new SelectList(brandList, brand);

            ViewBag.SearchKeyword = searchKeyword;
            ViewBag.SelectedRoomId = roomId;
            ViewBag.SelectedArea = areaFilter;
            ViewBag.SelectedBrand = brand;
            ViewBag.SelectedStatus = status;

            return View(equipments);
        }

        // GET: /Admin/EquipmentManagement/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var equipment = await _context.Equipments
                .Include(e => e.GymRoom)
                .Include(e => e.MaintenanceLogs.OrderByDescending(m => m.MaintenanceDate))
                .FirstOrDefaultAsync(e => e.EquipmentId == id);

            if (equipment == null)
            {
                SetErrorMessage("Không tìm thấy thiết bị yêu cầu.");
                return RedirectToAction(nameof(Index));
            }

            return View(equipment);
        }

        // GET: /Admin/EquipmentManagement/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            var vm = new EquipmentFormViewModel
            {
                PurchaseDate = DateTime.Today,
                Status = "Hoạt động tốt",
                Area = "Khu máy khối",
                Brand = "Technogym"
            };
            return View(vm);
        }

        // POST: /Admin/EquipmentManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EquipmentFormViewModel model, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(model.RoomId);
                return View(model);
            }

            string? finalImageUrl = model.ImageUrl;

            // Xử lý upload ảnh nếu có
            if (imageFile != null && imageFile.Length > 0)
            {
                var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                if (allowedExts.Contains(ext))
                {
                    var fileName = $"equipment_{Guid.NewGuid():N}{ext}";
                    var savePath = Path.Combine(_env.WebRootPath, "images", "equipments", fileName);
                    using var stream = new FileStream(savePath, FileMode.Create);
                    await imageFile.CopyToAsync(stream);
                    finalImageUrl = $"/images/equipments/{fileName}";
                }
            }

            var equipment = new Equipment
            {
                EquipmentName = model.EquipmentName.Trim(),
                Area = model.Area.Trim(),
                RoomId = model.RoomId,
                Brand = model.Brand.Trim(),
                PurchaseDate = model.PurchaseDate,
                LastMaintenanceDate = model.LastMaintenanceDate,
                Status = model.Status,
                SerialNumber = model.SerialNumber?.Trim(),
                ImageUrl = finalImageUrl,
                Notes = model.Notes?.Trim()
            };

            _context.Equipments.Add(equipment);
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã thêm thiết bị '{equipment.EquipmentName}' thành công!");
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/EquipmentManagement/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment == null)
            {
                SetErrorMessage("Không tìm thấy thiết bị cần sửa.");
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(equipment.RoomId);

            var vm = new EquipmentFormViewModel
            {
                EquipmentId = equipment.EquipmentId,
                EquipmentName = equipment.EquipmentName,
                Area = equipment.Area,
                RoomId = equipment.RoomId,
                Brand = equipment.Brand,
                PurchaseDate = equipment.PurchaseDate,
                LastMaintenanceDate = equipment.LastMaintenanceDate,
                Status = equipment.Status,
                SerialNumber = equipment.SerialNumber,
                ImageUrl = equipment.ImageUrl,
                Notes = equipment.Notes
            };

            return View(vm);
        }

        // POST: /Admin/EquipmentManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EquipmentFormViewModel model, IFormFile? imageFile)
        {
            if (id != model.EquipmentId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(model.RoomId);
                return View(model);
            }

            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment == null)
            {
                SetErrorMessage("Không tìm thấy thiết bị.");
                return RedirectToAction(nameof(Index));
            }

            string? finalImageUrl = model.ImageUrl;

            if (imageFile != null && imageFile.Length > 0)
            {
                var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                if (allowedExts.Contains(ext))
                {
                    var fileName = $"equipment_{Guid.NewGuid():N}{ext}";
                    var savePath = Path.Combine(_env.WebRootPath, "images", "equipments", fileName);
                    using var stream = new FileStream(savePath, FileMode.Create);
                    await imageFile.CopyToAsync(stream);
                    finalImageUrl = $"/images/equipments/{fileName}";
                }
            }

            equipment.EquipmentName = model.EquipmentName.Trim();
            equipment.Area = model.Area.Trim();
            equipment.RoomId = model.RoomId;
            equipment.Brand = model.Brand.Trim();
            equipment.PurchaseDate = model.PurchaseDate;
            equipment.LastMaintenanceDate = model.LastMaintenanceDate;
            equipment.Status = model.Status;
            equipment.SerialNumber = model.SerialNumber?.Trim();
            equipment.ImageUrl = finalImageUrl;
            equipment.Notes = model.Notes?.Trim();

            await _context.SaveChangesAsync();

            SetSuccessMessage($"Cập nhật thiết bị '{equipment.EquipmentName}' thành công!");
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/EquipmentManagement/QuickChangeStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuickChangeStatus(int id, string newStatus, string? returnUrl)
        {
            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Không tìm thấy thiết bị." });
                }
                SetErrorMessage("Không tìm thấy thiết bị.");
                return RedirectToAction(nameof(Index));
            }

            var oldStatus = equipment.Status;
            equipment.Status = newStatus;

            // Nếu đổi sang "Đang hỏng" hoặc "Cần bảo trì", tự động tạo 1 log bảo trì sơ bộ nếu chưa có phiếu pending
            if (newStatus == "Đang hỏng" || newStatus == "Cần bảo trì")
            {
                var hasActiveLog = await _context.MaintenanceLogs
                    .AnyAsync(m => m.EquipmentId == id && m.Status != "Hoàn thành");

                if (!hasActiveLog)
                {
                    _context.MaintenanceLogs.Add(new MaintenanceLog
                    {
                        EquipmentId = id,
                        MaintenanceDate = DateTime.UtcNow,
                        Description = $"Ghi nhận sự cố: Thiết bị chuyển sang trạng thái '{newStatus}'.",
                        Cost = 0,
                        PerformedBy = "Chờ phân công kỹ thuật",
                        Status = newStatus == "Đang hỏng" ? "Chờ linh kiện" : "Đang bảo dưỡng",
                        Notes = "Tự động ghi nhận khi Admin cập nhật trạng thái nhanh."
                    });
                }
            }

            await _context.SaveChangesAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { 
                    success = true, 
                    message = $"Đã cập nhật trạng thái thiết bị sang '{newStatus}'!",
                    status = newStatus 
                });
            }

            SetSuccessMessage($"Đã đổi trạng thái '{equipment.EquipmentName}' sang '{newStatus}'!");
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/EquipmentManagement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment == null)
            {
                SetErrorMessage("Không tìm thấy thiết bị.");
                return RedirectToAction(nameof(Index));
            }

            _context.Equipments.Remove(equipment);
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã xóa thiết bị '{equipment.EquipmentName}' thành công!");
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdowns(int? selectedRoomId = null)
        {
            var rooms = await _context.GymRooms.OrderBy(r => r.RoomName).ToListAsync();
            ViewBag.RoomsList = new SelectList(rooms, "RoomId", "RoomName", selectedRoomId);
        }
    }
}

