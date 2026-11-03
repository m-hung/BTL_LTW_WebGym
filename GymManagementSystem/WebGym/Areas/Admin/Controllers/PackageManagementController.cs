using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Admin;
using WebGym.Models.Entities;

namespace WebGym.Areas.Admin.Controllers
{
    public class PackageManagementController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;

        public PackageManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/PackageManagement
        public async Task<IActionResult> Index(string? searchKeyword)
        {
            var query = _context.GymPackages
                .Include(p => p.Subscriptions)
                .Include(p => p.Payments)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                query = query.Where(p => p.PackageName.Contains(searchKeyword) ||
                                         (p.Description != null && p.Description.Contains(searchKeyword)));
            }

            var packages = await query.OrderBy(p => p.Price).ToListAsync();
            ViewBag.SearchKeyword = searchKeyword;

            return View(packages);
        }

        // GET: /Admin/PackageManagement/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new GymPackageFormViewModel
            {
                DurationDays = 30,
                Price = 500000,
                IncludesPT = false
            });
        }

        // POST: /Admin/PackageManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GymPackageFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var package = new GymPackage
            {
                PackageName = model.PackageName.Trim(),
                Description = model.Description?.Trim(),
                Price = model.Price,
                DurationDays = model.DurationDays,
                IncludesPT = model.IncludesPT
            };

            _context.GymPackages.Add(package);
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã thêm mới gói tập '{package.PackageName}' thành công!");
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/PackageManagement/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var package = await _context.GymPackages.FindAsync(id);
            if (package == null)
            {
                SetErrorMessage("Không tìm thấy gói tập yêu cầu.");
                return RedirectToAction(nameof(Index));
            }

            var model = new GymPackageFormViewModel
            {
                PackageId = package.PackageId,
                PackageName = package.PackageName,
                Description = package.Description,
                Price = package.Price,
                DurationDays = package.DurationDays,
                IncludesPT = package.IncludesPT
            };

            return View(model);
        }

        // POST: /Admin/PackageManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GymPackageFormViewModel model)
        {
            if (id != model.PackageId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var package = await _context.GymPackages.FindAsync(id);
            if (package == null)
            {
                SetErrorMessage("Không tìm thấy gói tập để cập nhật.");
                return RedirectToAction(nameof(Index));
            }

            package.PackageName = model.PackageName.Trim();
            package.Description = model.Description?.Trim();
            package.Price = model.Price;
            package.DurationDays = model.DurationDays;
            package.IncludesPT = model.IncludesPT;

            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã cập nhật thông tin gói tập '{package.PackageName}'.");
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/PackageManagement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var package = await _context.GymPackages
                .Include(p => p.Subscriptions)
                .Include(p => p.Payments)
                .FirstOrDefaultAsync(p => p.PackageId == id);

            if (package == null)
            {
                SetErrorMessage("Không tìm thấy gói tập cần xóa.");
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra ràng buộc dữ liệu: Nếu đã có hội viên mua thì không cho xóa cứng để bảo đảm lịch sử tài chính
            if (package.Subscriptions.Any() || package.Payments.Any())
            {
                SetErrorMessage($"Gói tập '{package.PackageName}' đã có dữ liệu đăng ký hoặc thanh toán, không thể xóa để đảm bảo toàn vẹn dữ liệu!");
                return RedirectToAction(nameof(Index));
            }

            _context.GymPackages.Remove(package);
            await _context.SaveChangesAsync();

            SetSuccessMessage($"Đã xóa gói tập '{package.PackageName}' khỏi hệ thống.");
            return RedirectToAction(nameof(Index));
        }
    }
}

