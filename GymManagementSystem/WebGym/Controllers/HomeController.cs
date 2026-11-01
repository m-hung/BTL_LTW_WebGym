using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models;

namespace WebGym.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: /
        public async Task<IActionResult> Index()
        {
            // Landing page - Lấy danh sách 4 gói tập nổi bật
            var featuredPackages = await _context.GymPackages
                .OrderBy(p => p.Price)
                .Take(4)
                .ToListAsync();

            ViewBag.FeaturedPackages = featuredPackages;
            return View();
        }

        // GET: /Home/Packages
        public async Task<IActionResult> Packages()
        {
            var packages = await _context.GymPackages
                .OrderBy(p => p.Price)
                .ToListAsync();

            return View(packages);
        }

        // GET: /Home/Facilities
        public IActionResult Facilities()
        {
            return View();
        }

        // GET: /Home/About
        public IActionResult About()
        {
            return View();
        }

        // GET: /Home/Contact
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
