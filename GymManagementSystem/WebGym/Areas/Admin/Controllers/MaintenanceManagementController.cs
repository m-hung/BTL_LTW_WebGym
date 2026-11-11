using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Admin;
using WebGym.Models.Entities;

namespace WebGym.Areas.Admin.Controllers
{
    public class MaintenanceManagementController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/MaintenanceManagement
        public async Task<IActionResult> Index(
            string? status, 
            int? equipmentId, 
            int? year, 
            int? month)
        {
            var selectedYear = year ?? DateTime.UtcNow.Year;
            var now = DateTime.UtcNow;

            var query = _context.MaintenanceLogs
                .Include(m => m.Equipment)
                    .ThenInclude(e => e.GymRoom)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(m => m.Status == status);
            }

            if (equipmentId.HasValue && equipmentId.Value > 0)
            {
                query = query.Where(m => m.EquipmentId == equipmentId.Value);
            }

            if (year.HasValue && year.Value > 0)
            {
                query = query.Where(m => m.MaintenanceDate.Year == year.Value);
            }

            if (month.HasValue && month.Value > 0)
            {
                query = query.Where(m => m.MaintenanceDate.Month == month.Value);
            }

            var logs = await query.OrderByDescending(m => m.MaintenanceDate).ToListAsync();

            // Thống kê chi phí toàn diện
            var allCompletedLogs = await _context.MaintenanceLogs
                .Where(m => m.Status == "Hoàn thành")
                .ToListAsync();

            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            int currentQuarter = (now.Month - 1) / 3 + 1;
            var startOfQuarter = new DateTime(now.Year, (currentQuarter - 1) * 3 + 1, 1);

            var stats = new MaintenanceStatsSummary
            {
                TotalCostOverall = allCompletedLogs.Sum(m => m.Cost),
                CostThisMonth = allCompletedLogs.Where(m => m.MaintenanceDate >= startOfMonth).Sum(m => m.Cost),
                CostThisQuarter = allCompletedLogs.Where(m => m.MaintenanceDate >= startOfQuarter).Sum(m => m.Cost),
                TotalLogsCount = await _context.MaintenanceLogs.CountAsync(),
                PendingLogsCount = await _context.MaintenanceLogs.CountAsync(m => m.Status != "Hoàn thành"),
                CompletedLogsCount = allCompletedLogs.Count
            };

            // Thống kê 12 tháng trong năm được chọn
            for (int m = 1; m <= 12; m++)
            {
                var monthLogs = allCompletedLogs.Where(x => x.MaintenanceDate.Year == selectedYear && x.MaintenanceDate.Month == m).ToList();
                stats.MonthlyCosts.Add(new MonthlyCostItem
                {
                    Month = m,
                    Year = selectedYear,
                    Label = $"Tháng {m}/{selectedYear}",
                    Cost = monthLogs.Sum(x => x.Cost),
                    Count = monthLogs.Count
                });
            }

            ViewBag.Stats = stats;
            ViewBag.SelectedStatus = status;
            ViewBag.SelectedEquipmentId = equipmentId;
            ViewBag.SelectedYear = selectedYear;
            ViewBag.SelectedMonth = month;

            // Dropdown thiết bị
            var equipments = await _context.Equipments.OrderBy(e => e.EquipmentName).ToListAsync();
            ViewBag.EquipmentList = new SelectList(equipments, "EquipmentId", "EquipmentName", equipmentId);

            return View(logs);
        }

        // GET: /Admin/MaintenanceManagement/Create
        [HttpGet]
        public async Task<IActionResult> Create(int? equipmentId)
        {
            await PopulateEquipmentsDropdown(equipmentId);

            var vm = new MaintenanceLogFormViewModel
            {
                EquipmentId = equipmentId ?? 0,
                MaintenanceDate = DateTime.Today,
                Status = "Đang bảo dưỡng",
                PerformedBy = "Kỹ thuật bảo trì nội bộ",
                SyncEquipmentStatus = true
            };

            return View(vm);
        }

        // POST: /Admin/MaintenanceManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaintenanceLogFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateEquipmentsDropdown(model.EquipmentId);
                return View(model);
            }

            var log = new MaintenanceLog
            {
                EquipmentId = model.EquipmentId,
                MaintenanceDate = model.MaintenanceDate,
                Description = model.Description.Trim(),
                Cost = model.Cost,
                PerformedBy = model.PerformedBy.Trim(),
                Status = model.Status,
                Notes = model.Notes?.Trim()
            };

            _context.MaintenanceLogs.Add(log);

            // Đồng bộ trạng thái thiết bị
            if (model.SyncEquipmentStatus)
            {
                var equipment = await _context.Equipments.FindAsync(model.EquipmentId);
                if (equipment != null)
                {
                    if (model.Status == "Hoàn thành")
                    {
                        equipment.Status = "Hoạt động tốt";
                        equipment.LastMaintenanceDate = model.MaintenanceDate;
                    }
                    else if (model.Status == "Đang bảo dưỡng")
                    {
                        equipment.Status = "Cần bảo trì";
                    }
                    else if (model.Status == "Chờ linh kiện")
                    {
                        equipment.Status = "Đang hỏng";
                    }
                }
            }

            await _context.SaveChangesAsync();

            SetSuccessMessage("Tạo phiếu bảo trì thành công!");
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/MaintenanceManagement/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var log = await _context.MaintenanceLogs
                .Include(m => m.Equipment)
                .FirstOrDefaultAsync(m => m.LogId == id);

            if (log == null)
            {
                SetErrorMessage("Không tìm thấy phiếu bảo trì yêu cầu.");
                return RedirectToAction(nameof(Index));
            }

            await PopulateEquipmentsDropdown(log.EquipmentId);

            var vm = new MaintenanceLogFormViewModel
            {
                LogId = log.LogId,
                EquipmentId = log.EquipmentId,
                MaintenanceDate = log.MaintenanceDate,
                Description = log.Description,
                Cost = log.Cost,
                PerformedBy = log.PerformedBy,
                Status = log.Status,
                Notes = log.Notes,
                SyncEquipmentStatus = true
            };

            return View(vm);
        }

        // POST: /Admin/MaintenanceManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MaintenanceLogFormViewModel model)
        {
            if (id != model.LogId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await PopulateEquipmentsDropdown(model.EquipmentId);
                return View(model);
            }

            var log = await _context.MaintenanceLogs.FindAsync(id);
            if (log == null)
            {
                SetErrorMessage("Không tìm thấy phiếu bảo trì.");
                return RedirectToAction(nameof(Index));
            }

            log.EquipmentId = model.EquipmentId;
            log.MaintenanceDate = model.MaintenanceDate;
            log.Description = model.Description.Trim();
            log.Cost = model.Cost;
            log.PerformedBy = model.PerformedBy.Trim();
            log.Status = model.Status;
            log.Notes = model.Notes?.Trim();

            // Đồng bộ trạng thái thiết bị
            if (model.SyncEquipmentStatus)
            {
                var equipment = await _context.Equipments.FindAsync(model.EquipmentId);
                if (equipment != null)
                {
                    if (model.Status == "Hoàn thành")
                    {
                        equipment.Status = "Hoạt động tốt";
                        equipment.LastMaintenanceDate = model.MaintenanceDate;
                    }
                    else if (model.Status == "Đang bảo dưỡng")
                    {
                        equipment.Status = "Cần bảo trì";
                    }
                    else if (model.Status == "Chờ linh kiện")
                    {
                        equipment.Status = "Đang hỏng";
                    }
                }
            }

            await _context.SaveChangesAsync();

            SetSuccessMessage("Cập nhật phiếu bảo trì thành công!");
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/MaintenanceManagement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var log = await _context.MaintenanceLogs.FindAsync(id);
            if (log == null)
            {
                SetErrorMessage("Không tìm thấy phiếu bảo trì.");
                return RedirectToAction(nameof(Index));
            }

            _context.MaintenanceLogs.Remove(log);
            await _context.SaveChangesAsync();

            SetSuccessMessage("Đã xóa phiếu bảo trì thành công!");
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateEquipmentsDropdown(int? selectedId = null)
        {
            var equipments = await _context.Equipments
                .Include(e => e.GymRoom)
                .OrderBy(e => e.EquipmentName)
                .Select(e => new
                {
                    e.EquipmentId,
                    DisplayName = $"{e.EquipmentName} - [Serial: {e.SerialNumber ?? "N/A"}] ({(e.GymRoom != null ? e.GymRoom.RoomName : e.Area)})"
                })
                .ToListAsync();

            ViewBag.EquipmentsList = new SelectList(equipments, "EquipmentId", "DisplayName", selectedId);
        }
    }
}

