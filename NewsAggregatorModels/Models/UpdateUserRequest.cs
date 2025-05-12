
namespace NewsAggregatorModels.Models
{
    public class UpdateUserRequest
    {
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public string? NewPassword { get; set; }
    }
}
