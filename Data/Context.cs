using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ufoShopBack.Data.Entities;
using ufoShopBack.Data.Enums;
namespace ufoShopBack.Data
{
    public class Context : DbContext
    {
        private readonly IConfiguration _configuration;
        public Context(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("SqlServer");
            optionsBuilder.UseSqlServer(connectionString);
            
        } 
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RatingList> Rating { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Order>()
                .HasOne(p => p.User)
                .WithMany(pi => pi.Orders)
                .HasForeignKey(p => p.UserId);
            modelBuilder.Entity<LikedItem>()
                .HasOne(p => p.User)
                .WithMany(pi => pi.LikedItems)
                .HasForeignKey(p => p.UserId);
            modelBuilder.Entity<LikedItem>()
                .HasOne(p => p.Product)
                .WithMany(pi => pi.LikedItems)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(pi => pi.Products)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<Review>()
                .HasOne(p => p.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(c => c.ProductId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(p => p.Order)
                .WithMany(pi => pi.OrderItems)
                .HasForeignKey(o => o.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(p => p.Product)
                .WithMany(pi => pi.OrderItems)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(10, 2);
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new {ur.UserId, ur.RoleId});
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            //ENTERING DATA TO A ROLE TABLE
            var roles = Enum
                .GetValues<RoleEnum>()
                .Select(r => new Role
                {
                    Id = (int)r,
                    RoleName = r.ToString()
                })
                .ToArray();

            modelBuilder.Entity<Role>().HasData(roles);
        }

      
    }
}
