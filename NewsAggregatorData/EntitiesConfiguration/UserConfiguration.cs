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
            builder.HasAlternateKey(u => u.Login);
            builder.HasAlternateKey(u => u.Email);
        }
    }
}
