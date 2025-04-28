using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Tagerly.Models;

namespace Tagerly.DataAccess.ConfigurationClasses
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable("Profiles");  // Added explicit table name

            builder.HasKey(p => p.ProfileId);  // Added explicit primary key

            builder.Property(p => p.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Address)
                .HasMaxLength(200);

            builder.Property(p => p.BirthDate)
                .IsRequired(false);  // Explicitly marked BirthDate as optional

            // Added complete one-to-one relationship configuration
            builder.HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<Profile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}