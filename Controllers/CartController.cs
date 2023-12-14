using ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

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
        public IActionResult AddToCart(int productId, int quantity)
        {
            // Получите текущего пользователя
            var user = _context.Users.SingleOrDefault(u => u.Username == User.Identity.Name);

            // Проверьте, есть ли уже такой товар в корзине пользователя
var existingCartItem = _context.CartItems.SingleOrDefault(ci => ci.ProductId == productId && ci.UserId == user.UserId);


            if (existingCartItem != null)
            {
                // Если товар уже в корзине, увеличьте количество
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // Иначе, создайте новую запись в корзине
                var newCartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    User = user
                };

                _context.CartItems.Add(newCartItem);
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Home"); // или куда-то еще
        }
    }
}