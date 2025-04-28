using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tagerly.Models;

namespace Tagerly.DataAccess.ConfigurationClasses
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // علاقات الـ Navigation
            builder.HasOne(u => u.Profile)
                   .WithOne(p => p.User)
                   .HasForeignKey<ApplicationUser>(u => u.ProfileId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Cart)
                   .WithOne(c => c.User)
                   .HasForeignKey<ApplicationUser>(u => u.CartId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Orders)
                   .WithOne(o => o.User)
                   .HasForeignKey(o => o.UserId);

            builder.HasMany(u => u.Favourites)
                   .WithOne(f => f.User)
                   .HasForeignKey(f => f.UserId);

            builder.HasMany(u => u.Payments)
                   .WithOne(p => p.User)
                   .HasForeignKey(p => p.UserId);

            builder.HasMany(u => u.Notifications)
                   .WithOne(n => n.User)
                   .HasForeignKey(n => n.UserId);

            // خصائص إضافية
            builder.Property(u => u.Address).HasMaxLength(200);
        }
    }
}
