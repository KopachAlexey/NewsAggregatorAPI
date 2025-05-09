using NewsAggregatorData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NewsAggregatorData.EntitiesConfiguration
{
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(c => c.Id).HasField("_id");
            builder.Property(c => c.Text).HasMaxLength(1000);
        }
    }
}
