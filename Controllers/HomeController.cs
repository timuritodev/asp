using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASP.Models;
using Microsoft.Extensions.Logging; // Добавьте это пространство имен

namespace ASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Добавьте это поле

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) // Добавьте ApplicationDbContext в конструктор
        {
            _logger = logger;
            _context = context; // Инициализируйте поле контекста данных
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList(); // Получите список продуктов из базы данных
            return View(products); // Передайте список продуктов в представление
        }

        public IActionResult Privacy()
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
