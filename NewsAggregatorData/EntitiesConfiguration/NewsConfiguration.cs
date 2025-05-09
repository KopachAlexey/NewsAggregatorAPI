using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsAggregatorData.Entities;

namespace NewsAggregatorData.EntitiesConfiguration
{
    internal class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.Property(n => n.Id).HasField("_id");
            builder.Property(n => n.Topic).HasMaxLength(500);
            builder.Property(n => n.Headline).HasMaxLength(500);
            builder.Property(n => n.Url).HasMaxLength(3000);
            builder.Property(n => n.ImageUrl).HasMaxLength(3000);
            builder.ToTable(t => t.HasCheckConstraint("ValidRate", "PositivityRate >= -10 AND PositivityRate <= 10"));
            builder.HasAlternateKey(n => n.Url);
        }
    }
}
