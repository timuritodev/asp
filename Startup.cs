using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ASP.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ASP
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = "Server=localhost;Database=asp;User=root;Password=timur2003;";
            services.AddDbContext<ApplicationDbContext>(options =>
     options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 1, 0))));
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
           name: "default",
           pattern: "{controller=Home}/{action=Index}/{id?}");

   });

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
