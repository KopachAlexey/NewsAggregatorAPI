using NewsAggregatorCore.DTO;

namespace NewsAggregatorServices.Abstracts
{
    public interface IUserServices
    {
        Task<AddResourceResultDTO> AddNewUserAsync(UserDTO newUser);

        Task<UserDTO?> GetUserByLoginAsync(string login);

        Task<UserDTO?> GetUserByIdAsync(Guid id);

        Task<OperationResultDTO> DelUserByLoginAsync(string login);

        Task<OperationResultDTO> UpdateUserByIdAsync(Guid id, UpdateUserDTO newUserData);
        Task UpdateUserNewsRateByIdAsync(Guid id, double newNewsRate);
    }
}
