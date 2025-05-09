using NewsAggregatorCore;

namespace NewsAggregatorData.Entities
{
    public class Role
    {
        private int _id;
        public int Id => _id;
        public string RoleName { get; set; }
        public List<User> Users { get; set; } = new();
    }
}