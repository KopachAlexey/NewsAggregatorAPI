namespace NewsAggregatorCore.DTO
{
    public class UpdateUserDTO
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string? NewLogin { get; set; }
        public string? NewEmail { get; set; }
        public string PasswordHash { get; set; }
    }
}
