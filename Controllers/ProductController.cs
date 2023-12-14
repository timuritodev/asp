using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using ASP.Models;
using Microsoft.Extensions.Logging;

namespace ASP.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ApplicationDbContext context, ILogger<ProductController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("Attempting to fetch products from the database.");

                var products = _context.Products.ToList();

                _logger.LogInformation($"Found {products.Count} products.");

                return View(products); // Вот этот код важен, чтобы передать данные в представление.
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching products: {ex.Message}");
                throw;
            }
        }

        public IActionResult List()
        {
            try
            {
                _logger.LogInformation("Attempting to fetch products from the database for the List action.");

                var products = _context.Products.ToList();

                _logger.LogInformation($"Found {products.Count} products for the List action.");

                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching products for the List action: {ex.Message}");
                throw;
            }
        }
    }
}