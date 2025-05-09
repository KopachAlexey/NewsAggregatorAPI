namespace NewsAggregatorModels.Models
{
    public class AddUserRequest
    {
        public string Password{ get; init; }
        public string Login { get; init; }
        public string Email { get; init; }
    }
}
