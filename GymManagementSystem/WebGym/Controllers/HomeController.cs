using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebGym.Data;
using WebGym.Models;
using WebGym.Models.Entities;

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
            // Landing page - Lấy danh sách gói tập & HLV từ Database
            var featuredPackages = await _context.GymPackages
                .OrderBy(p => p.Price)
                .ToListAsync();

            var trainers = await _context.Trainers
                .Include(t => t.User)
                .ToListAsync();

            ViewBag.FeaturedPackages = featuredPackages;
            ViewBag.Trainers = trainers;
            return View(featuredPackages);
        }

        // GET: /khu-vuc/khu-ta-tu-do  (khu-ta-tu-do | khu-cardio | phong-boxing | studio-yoga)
        [Route("khu-vuc/{id}")]
        public async Task<IActionResult> FacilityDetail(string id)
        {
            var facility = FacilityData.Find(id);
            if (facility == null) return NotFound();

            var trainers = await _context.Trainers
                .Include(t => t.User)
                .ToListAsync();

            var allTrainerInfos = trainers.Select(TrainerInfo.FromTrainer).ToList();
            ViewBag.Coaches = allTrainerInfos.Where(t => t.AreaSlugs.Contains(id)).ToList();

            return View(facility);
        }

        // GET: /huan-luyen-vien/1  hoặc /huan-luyen-vien/alex-tokuda
        [Route("huan-luyen-vien/{id}")]
        public async Task<IActionResult> TrainerDetail(string id)
        {
            var trainers = await _context.Trainers
                .Include(t => t.User)
                .ToListAsync();

            Trainer? trainer = null;
            if (int.TryParse(id, out int tid))
            {
                trainer = trainers.FirstOrDefault(t => t.TrainerId == tid);
            }
            else if (!string.IsNullOrEmpty(id))
            {
                trainer = trainers.FirstOrDefault(t =>
                    (t.User?.Username != null && t.User.Username.Contains(id, StringComparison.OrdinalIgnoreCase))
                    || (t.User?.FullName != null && t.User.FullName.Contains(id, StringComparison.OrdinalIgnoreCase))
                    || (id.Equals("alex-tokuda", StringComparison.OrdinalIgnoreCase) && t.TrainerId == 1)
                    || (id.Equals("mina-tran", StringComparison.OrdinalIgnoreCase) && t.TrainerId == 2)
                    || (id.Equals("lil-baby", StringComparison.OrdinalIgnoreCase) && t.TrainerId == 3));
            }

            trainer ??= trainers.FirstOrDefault();

            if (trainer == null)
            {
                return NotFound("Không tìm thấy thông tin huấn luyện viên.");
            }

            var allTrainerInfos = trainers.Select(TrainerInfo.FromTrainer).ToList();
            ViewBag.AllTrainers = allTrainerInfos;

            var model = TrainerInfo.FromTrainer(trainer);
            return View(model);
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
