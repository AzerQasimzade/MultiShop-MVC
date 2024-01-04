using Microsoft.EntityFrameworkCore;
using MultiShop.Models;

namespace MultiShop.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
            
        }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Productlar> Productlar { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductColor> ProductColor { get; set; }
    }
}
