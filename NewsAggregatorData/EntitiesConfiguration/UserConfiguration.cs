using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsAggregatorData.Entities;

namespace NewsAggregatorData.EntitiesConfiguration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Id).HasField("_id");
            builder.Property(u => u.PasswordHash).HasColumnType("nvarchar");
            builder.Property(u => u.PasswordHash).HasMaxLength(100);
            builder.Property(u => u.Login).HasMaxLength(30);
            builder.Property(u => u.Email).HasColumnType("nvarchar");
            builder.Property(u => u.Email).HasMaxLength(100);
            builder.Property(u => u.NewsMinRate).HasDefaultValue(0);
            builder.ToTable(t => t.HasCheckConstraint("ValidNewsMinRate", "NewsMinRate >= -5 AND NewsMinRate <= 5"));
            builder.HasIndex(u => u.Login).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}
