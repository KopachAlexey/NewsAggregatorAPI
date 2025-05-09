using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsAggregatorData.Entities;

namespace NewsAggregatorData.EntitiesConfiguration
{
    class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder.Property(r => r.Id).HasField("_id");
            builder.Property(r => r.ReactionName).HasColumnType("nvarchar");
            builder.Property(r => r.ReactionName).HasMaxLength(30);
            builder.HasAlternateKey(r => r.ReactionName);
        }
    }
}
