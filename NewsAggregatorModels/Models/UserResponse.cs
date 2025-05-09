namespace NewsAggregatorModels.Models
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}
