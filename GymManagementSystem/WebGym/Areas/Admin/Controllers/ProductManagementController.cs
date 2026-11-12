using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models.DTOs.Admin;
using WebGym.Models.Entities;

namespace WebGym.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductManagementController : AdminBaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductManagementController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: /Admin/ProductManagement
        public async Task<IActionResult> Index(
            [FromQuery(Name = "categoryFilter")] int? categoryFilter, 
            [FromQuery(Name = "stockFilter")] string? stockFilter, 
            [FromQuery(Name = "keyword")] string? keyword)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (categoryFilter.HasValue && categoryFilter.Value > 0)
            {
                query = query.Where(p => p.CategoryId == categoryFilter.Value);
            }

            if (!string.IsNullOrWhiteSpace(stockFilter))
            {
                switch (stockFilter.ToLower())
                {
                    case "low":
                        query = query.Where(p => p.StockQuantity > 0 && p.StockQuantity < 5);
                        break;
                    case "out":
                        query = query.Where(p => p.StockQuantity <= 0);
                        break;
                    case "instock":
                        query = query.Where(p => p.StockQuantity >= 5);
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword.Trim().ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(kw) || 
                                         (p.Description != null && p.Description.ToLower().Contains(kw)));
            }

            var products = await query.OrderByDescending(p => p.ProductId).ToListAsync();
            var allProducts = await _context.Products.AsNoTracking().ToListAsync();
            var categories = await _context.ProductCategories.OrderBy(c => c.CategoryName).ToListAsync();

            var viewModel = new ProductFilterViewModel
            {
                CategoryId = categoryFilter,
                StockStatus = stockFilter,
                SearchKeyword = keyword,
                Products = products,
                Categories = categories,
                TotalProducts = allProducts.Count,
                LowStockCount = allProducts.Count(p => p.StockQuantity > 0 && p.StockQuantity < 5),
                OutOfStockCount = allProducts.Count(p => p.StockQuantity <= 0),
                TotalInventoryValue = allProducts.Sum(p => p.Price * p.StockQuantity)
            };

            return View(viewModel);
        }

        // GET: /Admin/ProductManagement/Create
        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesDropDownList();
            return View(new ProductFormViewModel { IsActive = true, StockQuantity = 10, Price = 100000 });
        }

        // POST: /Admin/ProductManagement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? imageUrl = model.ImageUrl;

                // Handle file upload
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageUrl = await SaveUploadedImage(model.ImageFile);
                }

                var product = new Product
                {
                    CategoryId = model.CategoryId,
                    ProductName = model.ProductName.Trim(),
                    Price = model.Price,
                    StockQuantity = model.StockQuantity,
                    Description = model.Description?.Trim(),
                    ImageUrl = imageUrl,
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đã thêm sản phẩm '{product.ProductName}' thành công!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateCategoriesDropDownList(model.CategoryId);
            return View(model);
        }

        // GET: /Admin/ProductManagement/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm yêu cầu.";
                return RedirectToAction(nameof(Index));
            }

            var model = new ProductFormViewModel
            {
                ProductId = product.ProductId,
                CategoryId = product.CategoryId,
                ProductName = product.ProductName,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive
            };

            await PopulateCategoriesDropDownList(product.CategoryId);
            return View(model);
        }

        // POST: /Admin/ProductManagement/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductFormViewModel model)
        {
            if (id != model.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                    return RedirectToAction(nameof(Index));
                }

                string? imageUrl = product.ImageUrl;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageUrl = await SaveUploadedImage(model.ImageFile);
                }
                else if (!string.IsNullOrWhiteSpace(model.ImageUrl))
                {
                    imageUrl = model.ImageUrl.Trim();
                }

                product.CategoryId = model.CategoryId;
                product.ProductName = model.ProductName.Trim();
                product.Price = model.Price;
                product.StockQuantity = model.StockQuantity;
                product.Description = model.Description?.Trim();
                product.ImageUrl = imageUrl;
                product.IsActive = model.IsActive;

                _context.Update(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Cập nhật sản phẩm '{product.ProductName}' thành công!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateCategoriesDropDownList(model.CategoryId);
            return View(model);
        }

        // POST: /Admin/ProductManagement/ToggleStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.IsActive = !product.IsActive;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã cập nhật trạng thái của '{product.ProductName}'.";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: /Admin/ProductManagement/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.OrderDetails)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm cần xóa.";
                return RedirectToAction(nameof(Index));
            }

            if (product.OrderDetails.Any())
            {
                // Sản phẩm đã có đơn hàng -> chuyển sang ngưng kinh doanh để bảo toàn lịch sử hóa đơn
                product.IsActive = false;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Sản phẩm '{product.ProductName}' đã có lịch sử đơn hàng nên được chuyển sang trạng thái 'Ngưng kinh doanh' thay vì xóa hoàn toàn.";
            }
            else
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã xóa sản phẩm '{product.ProductName}' thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/ProductManagement/Categories
        public async Task<IActionResult> Categories()
        {
            var categories = await _context.ProductCategories
                .Include(c => c.Products)
                .OrderBy(c => c.CategoryId)
                .ToListAsync();

            return View(categories);
        }

        // POST: /Admin/ProductManagement/CreateCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryFormViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.CategoryName))
            {
                bool isActive = model.IsActive;
                if (Request.Form.ContainsKey("IsActive"))
                {
                    var val = Request.Form["IsActive"].ToString();
                    isActive = val.Contains("true", StringComparison.OrdinalIgnoreCase) || val.Equals("on", StringComparison.OrdinalIgnoreCase);
                }

                var category = new ProductCategory
                {
                    CategoryName = model.CategoryName.Trim(),
                    Description = model.Description?.Trim(),
                    IsActive = isActive
                };
                _context.ProductCategories.Add(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã thêm danh mục '{category.CategoryName}' thành công!";
            }
            return RedirectToAction(nameof(Categories));
        }

        // POST: /Admin/ProductManagement/EditCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(CategoryFormViewModel model)
        {
            var category = await _context.ProductCategories.FindAsync(model.CategoryId);
            if (category != null && !string.IsNullOrWhiteSpace(model.CategoryName))
            {
                category.CategoryName = model.CategoryName.Trim();
                category.Description = model.Description?.Trim();

                bool isActive = model.IsActive;
                if (Request.Form.ContainsKey("IsActive"))
                {
                    var val = Request.Form["IsActive"].ToString();
                    isActive = val.Contains("true", StringComparison.OrdinalIgnoreCase) || val.Equals("on", StringComparison.OrdinalIgnoreCase);
                }
                category.IsActive = isActive;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã cập nhật danh mục '{category.CategoryName}' thành công!";
            }
            return RedirectToAction(nameof(Categories));
        }

        // POST: /Admin/ProductManagement/DeleteCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.ProductCategories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy danh mục cần xóa.";
                return RedirectToAction(nameof(Categories));
            }

            if (category.Products.Any())
            {
                TempData["ErrorMessage"] = $"Không thể xóa danh mục '{category.CategoryName}' vì vẫn còn {category.Products.Count} sản phẩm trực thuộc!";
            }
            else
            {
                _context.ProductCategories.Remove(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã xóa danh mục '{category.CategoryName}' thành công!";
            }

            return RedirectToAction(nameof(Categories));
        }

        // Helpers
        private async Task PopulateCategoriesDropDownList(object? selectedCategory = null)
        {
            var categories = await _context.ProductCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

            ViewBag.CategoriesList = new SelectList(categories, "CategoryId", "CategoryName", selectedCategory);
        }

        private async Task<string> SaveUploadedImage(IFormFile file)
        {
            var uploadDir = Path.Combine(_env.WebRootPath, "images", "products");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"prod_{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploadDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/images/products/{fileName}";
        }
    }
}

