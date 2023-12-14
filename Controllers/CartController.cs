using ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ASP.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Если пользователь не аутентифицирован, вернуть представление с сообщением
                TempData["ErrorMessage"] = "Please log in or register to add items to your cart.";
                return RedirectToAction("Index", "Home");
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
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
