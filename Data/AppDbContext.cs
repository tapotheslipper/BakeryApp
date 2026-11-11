using Microsoft.EntityFrameworkCore;
using BakeryApp.Models;

namespace BakeryApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Product>().Property(p => p.Price).HasPrecision(10, 2);
            b.Entity<Order>().Property(o => o.TotalSum).HasPrecision(10, 2);
            b.Entity<OrderItem>().Property(i => i.Price).HasPrecision(10, 2);
        }
    }
}