using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tagerly.Models;
using Tagerly.DataAccess.ConfigurationClasses;

namespace Tagerly.DataAccess
{
    public class TagerlyDbContext : IdentityDbContext<ApplicationUser>
    {
        public TagerlyDbContext(DbContextOptions<TagerlyDbContext> options)
            : base(options)
        {
        }

        public TagerlyDbContext() : base()
        {

        }
        // DbSets
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations from the ConfigurationClasses namespace
            modelBuilder.ApplyConfiguration(new ProfileConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new FavouriteConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            //// Configure decimal precision globally
            //foreach (var property in modelBuilder.Model.GetEntityTypes()
            //    .SelectMany(t => t.GetProperties())
            //    .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            //{
            //    property.SetColumnType("decimal(18,2)");
            //}

            //// Configure Cart-Product many-to-many relationship
            //modelBuilder.Entity<Cart>()
            //    .HasMany(c => c.Products)
            //    .WithMany(p => p.Cart)
            //    .UsingEntity<Dictionary<string, object>>(
            //        "CartProduct",
            //        j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
            //        j => j.HasOne<Cart>().WithMany().HasForeignKey("CartId"),
            //        j => j.ToTable("CartProducts")
            //    );
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        // Development fallback configuration
        //        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TagerlyDB;Trusted_Connection=True;")
        //            .EnableSensitiveDataLogging()
        //            .EnableDetailedErrors();
        //    }
        //}
    }
}