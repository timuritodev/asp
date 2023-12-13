using ASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace ASP.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            // var countryList = GetCountryList();

            var model = new RegisterViewModel
            {
                // CountryList = countryList
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult RegistrationSuccess()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            _logger.LogInformation("Register action called.");

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Username = model.Username,
                    PasswordHash = model.Password,
                    Email = model.Email,
                    SelectedCountry = model.SelectedCountry,
                    Gender = model.Gender
                };

                // Добавим дополнительные логи
                _logger.LogInformation($"Registering user: Username={user.Username}, Email={user.Email}, Country={user.SelectedCountry}, Gender={user.Gender}");

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("RegistrationSuccess", "Account");
            }
            else
            {
                // Выводим ошибки валидации в лог
                _logger.LogInformation("Model state is not valid.");
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        _logger.LogError($"Validation error for {key}: {error.ErrorMessage}");
                    }
                }
            }

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

        // Вспомогательный метод для получения списка стран
        private List<SelectListItem> GetCountryList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "USA", Text = "United States" },
                new SelectListItem { Value = "Canada", Text = "Canada" },
                new SelectListItem { Value = "UK", Text = "United Kingdom" },
                new SelectListItem { Value = "Germany", Text = "Germany" },
                new SelectListItem { Value = "France", Text = "France" },
                new SelectListItem { Value = "Australia", Text = "Australia" },
                new SelectListItem { Value = "Japan", Text = "Japan" },
                new SelectListItem { Value = "China", Text = "China" },
                new SelectListItem { Value = "Brazil", Text = "Brazil" },
                new SelectListItem { Value = "India", Text = "India" }
            };
        }
    }
}
