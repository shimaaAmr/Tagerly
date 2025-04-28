using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tagerly.Models;

namespace Tagerly.DataAccess.ConfigurationClasses
{
    public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
    {
        public void Configure(EntityTypeBuilder<Favourite> builder)
        {
            // Composite key configuration (if needed)
            builder.HasKey(f => f.Id);

            // Many-to-One with ApplicationUser
            builder.HasOne(f => f.User)
                   .WithMany(u => u.Favourites)
                   .HasForeignKey(f => f.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Many-to-One with Product
            builder.HasOne(f => f.Product)
                   .WithMany(p => p.Favourites)
                   .HasForeignKey(f => f.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}