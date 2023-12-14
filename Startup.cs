using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ASP.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ASP
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = "Server=localhost;Database=asp;User=root;Password=timur2003;";
            services.AddDbContext<ApplicationDbContext>(options =>
                options
                    .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 1, 0)))
                    .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                    .EnableSensitiveDataLogging());

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.Cookie.Name = "YourAuthCookieName";
                });

            services.AddAuthorization();

            services.AddControllersWithViews();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Разместите вызовы ниже после UseRouting и перед UseEndpoints
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
               {
                   endpoints.MapControllerRoute(
                       name: "register",
                       pattern: "account/register",
                       defaults: new { controller = "Account", action = "Register" }
                   );

                   endpoints.MapControllerRoute(
                        name: "login",
                        pattern: "account/login",
                        defaults: new { controller = "Account", action = "Login" }
                    );

                   endpoints.MapControllerRoute(
                        name: "registrationSuccess",
                        pattern: "account/registrationsuccess",
                        defaults: new { controller = "Account", action = "RegistrationSuccess" }
                    );

                   endpoints.MapControllerRoute(
                       name: "loginSuccess",
                       pattern: "account/loginsuccess",
                       defaults: new { controller = "Account", action = "LoginSuccess" }
                   );

                   endpoints.MapControllerRoute(
                        name: "product",
                        pattern: "product/{action=Index}/{id?}",
                        defaults: new { controller = "Product" }
                    );
                   endpoints.MapControllerRoute(
                       name: "productList",
                       pattern: "products/list",
                       defaults: new { controller = "Product", action = "List" }
                   );
                   endpoints.MapControllerRoute(
                        name: "cart",
                        pattern: "cart",
                        defaults: new { controller = "Cart", action = "Index" }
                    );
                   endpoints.MapControllerRoute(
                        name: "logout",
                        pattern: "/Account/Logout",
                        defaults: new { controller = "Account", action = "Logout" }
                    );
                   endpoints.MapControllerRoute(
                       name: "default",
                       pattern: "{controller=Home}/{action=Index}/{id?}");
               });

        }
    }
}
