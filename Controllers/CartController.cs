using ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ASP.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CartController> _logger;

        public CartController(ApplicationDbContext context, ILogger<CartController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                var cartItemCount = context.HttpContext.Session.GetInt32("CartItemCount") ?? 0;
                ViewData["CartItemCount"] = cartItemCount;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error accessing session: {ex.Message}");
            }

            base.OnActionExecuting(context);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "Please log in or register to add items to your cart.";
                return RedirectToAction("Index", "Home");
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == User.Identity.Name);

            // Проверка наличия пользователя и получение его идентификатора
            if (user == null)
            {
                _logger.LogError("User not found.");
                return RedirectToAction("Index", "Home");
            }

            var existingCartItem = _context.CartItems.SingleOrDefault(ci => ci.ProductId == productId && ci.UserId == user.UserId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
            }
            else
            {
                var newCartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    User = user
                };

                _context.CartItems.Add(newCartItem);
            }

            _context.SaveChanges();

            try
            {
                // Обновление счетчика товаров в сессии
                var cartItemCount = HttpContext.Session.GetInt32("CartItemCount") ?? 0;
                cartItemCount += quantity;
                HttpContext.Session.SetInt32("CartItemCount", cartItemCount);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating session: {ex.Message}");
            }

            return RedirectToAction("Index", "Home");
        }

        private int GetUserId()
        {
            var userId = _context.Users.FirstOrDefault(u => u.Username == User.Identity.Name)?.UserId;
            return userId ?? 0;
        }

        public IActionResult Index()
        {
            var userId = GetUserId();
            var cartItems = _context.CartItems
                .Include(ci => ci.Product)
                .Where(c => c.UserId == userId)
                .ToList();

            return View(cartItems);
        }
    }
}