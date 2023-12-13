using ASP.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole(); // Это добавляет вывод в консоль
});

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql("Server=localhost;Database=asp;User=root;Password=timur2003;",
        new MySqlServerVersion(new Version(8, 1, 0))));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
