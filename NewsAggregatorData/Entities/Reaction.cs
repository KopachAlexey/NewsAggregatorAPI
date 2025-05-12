namespace NewsAggregatorData.Entities
{
    public class Reaction
    {
        private Guid _id;

        public Guid Id => _id;
        public string Name { get; set; }
        public List<UserCommentReaction> UserCommentReactions { get; set; }
    }
}
