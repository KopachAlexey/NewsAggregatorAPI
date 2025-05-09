namespace NewsAggregatorCore.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string PasswordHash { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public RoleDTO? Role { get; set; }
    }
}
