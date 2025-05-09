namespace NewsAggregatorCore.DTO
{
    public class NewUserDTO
    {
        public string PasswordHash { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
    }
}
