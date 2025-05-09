using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsAggregatorData.Entities;

namespace NewsAggregatorData.EntitiesConfiguration
{
    internal class SourceConfiguration : IEntityTypeConfiguration<Source>
    {
        public void Configure(EntityTypeBuilder<Source> builder)
        {
            builder.Property(s => s.Id).HasField("_id");
            builder.Property(s => s.Name).HasMaxLength(100);
            builder.Property(s => s.RssUrl).HasMaxLength(3000);
            builder.HasAlternateKey(s => new {s.Name, s.RssUrl});
        }
    }
}
