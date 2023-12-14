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

        private int GetUserId()
        {
            // Ваш код для получения Id текущего пользователя
            // Пример: если у вас есть свойство UserId в классе User, и текущий пользователь аутентифицирован
            var userId = _context.Users.FirstOrDefault(u => u.Username == User.Identity.Name)?.UserId;
            return userId ?? 0; // Если UserId не найден, возвращаем 0 (или другое значение по умолчанию)
        }
        
        public IActionResult Index()
        {
            // Получите данные о корзине из базы данных
            var userId = GetUserId(); // Реализуйте метод для получения Id текущего пользователя
            var cartItems = _context.CartItems
                .Include(ci => ci.Product) // Подгрузите связанные продукты
                .Where(c => c.UserId == userId)
                .ToList();

            return View(cartItems);
        }
    }
}