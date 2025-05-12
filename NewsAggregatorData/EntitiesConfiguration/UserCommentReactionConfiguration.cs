using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsAggregatorData.Entities;

namespace NewsAggregatorData.EntitiesConfiguration
{
    class UserCommentReactionConfiguration : IEntityTypeConfiguration<UserCommentReaction>
    {
        public void Configure(EntityTypeBuilder<UserCommentReaction> builder)
        {
            builder.Property(u => u.Id).HasField("_id");
            builder.HasIndex(u => new { u.UserId, u.CommentId }).IsUnique();
            builder
                .HasOne(u => u.Reaction)
                .WithMany(u => u.UserCommentReactions)
                .HasForeignKey(u => u.ReactionId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(u => u.User)
                .WithMany(u => u.UserCommentReactions)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(u => u.Comment)
                .WithMany(c => c.UserCommentReactions)
                .HasForeignKey(u => u.CommentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
