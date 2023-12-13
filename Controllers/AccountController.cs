using ASP.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegistrationSuccess()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Преобразуйте модель представления в объект пользователя
                var user = new User
                {
                    Username = model.Username,
                    PasswordHash = model.Password,
                    Email = model.Email
                    // Другие свойства пользователя
                };

                // Добавьте пользователя в контекст базы данных
                _context.Users.Add(user);

                // Сохраните изменения в базе данных
                _context.SaveChanges();

                // Редирект на страницу успешной регистрации или другую страницу
                // return RedirectToAction("RegistrationSuccess");
                return RedirectToAction("RegistrationSuccess", "Account");
            }

            // Если ModelState не валиден, верните представление с ошибками
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.PasswordHash == model.Password);

                if (user != null)
                {
                    // Успешный вход. Выполните необходимые действия, например, установка куки аутентификации и т. д.
                    // Пример:
                    // await HttpContext.SignInAsync(user.Username, model.RememberMe);

                    return RedirectToAction("LoginSuccess");
                }

                ModelState.AddModelError("", "Invalid login attempt");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult LoginSuccess()
        {
            return View();
        }
    }
}
