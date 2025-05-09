using Microsoft.EntityFrameworkCore;
using NewsAggregatorData.Entities;
using NewsAggregatorData.EntitiesConfiguration;

namespace NewsAggregatorData
{
    public class NewsAggregatorContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<UserCommentReaction> UserCommentReactions { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public NewsAggregatorContext(DbContextOptions<NewsAggregatorContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new NewsConfiguration());
            modelBuilder.ApplyConfiguration(new SourceConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new ReactionConfiguration());
            modelBuilder.ApplyConfiguration(new UserCommentReactionConfiguration());
        }
    }
}
