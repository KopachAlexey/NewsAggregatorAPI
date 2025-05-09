using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsAggregatorData.Entities;

namespace NewsAggregatorData.EntitiesConfiguration
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(r => r.Id).HasField("_id");
            builder.Property(r => r.RoleName).HasMaxLength(100);
            builder.Property(r => r.RoleName).HasColumnType("nvarchar");
            builder.HasAlternateKey(r => r.RoleName);
        }
    }
}
