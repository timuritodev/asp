using Microsoft.EntityFrameworkCore;

namespace ASP.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Добавьте здесь DbSet для ваших сущностей
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
