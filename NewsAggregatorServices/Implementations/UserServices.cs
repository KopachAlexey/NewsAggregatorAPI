using MediatR;
using NewsAggregatorCore;
using NewsAggregatorCore.DTO;
using NewsAggregatorCQS.Commands;
using NewsAggregatorCQS.Querys;
using NewsAggregatorModels.Models;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class UserServices : IUserServices
    {
        readonly IMediator _mediator;

        public UserServices(IMediator mediator, IPasswordHashing passwordHashing)
        {
            _mediator = mediator;
        }

        public async Task<AddResourceResultDTO> AddNewUserAsync(UserDTO newUser)
        {
            Guid newUserId = default;
            var userPreliminaryResult = await GetPreliminaryResultAsync(newUser);
            if (userPreliminaryResult.IsSuccessful)
            {
                var role = await _mediator.Send(new GetRoleByNameQuery { RoleName = NewsAggregatorConstants.UserRole });
                if (role is null)
                    throw new Exception(NewsAggregatorMessages.UserRoleNotExist);
                newUser.RoleId = role.Id;
                newUserId = await _mediator.Send(new AddUserCommand { NewUser = newUser });
            }
            return new AddResourceResultDTO
            {
                Id = newUserId,
                OperationResult = userPreliminaryResult
            };
        }

        public async Task<OperationResultDTO> DelUserByLoginAsync(string login)
        {
            bool isSuccessful = true;
            var error = String.Empty;
            var messages = new List<string>();
            var fields = new List<string>();
            if ((await GetUserByLoginAsync(login)) is null)
            {
                isSuccessful = false;
                error = OperationErrorsEnum.USER_DOES_NOT_EXIST.ToString();
                messages.Add(NewsAggregatorMessages.UserDoesNotExist);
                fields.Add(OperationFieldsEnum.Login.ToString());
            }
            else
                await _mediator.Send(new DelUserByLoginCommand { });
            return new OperationResultDTO
            {
                IsSuccessful = isSuccessful,
                Error = error,
                Messages = messages,
                Filds = fields
            };
        }

        public async Task<UserDTO?> GetUserByIdAsync(Guid id)
        {
            return await _mediator.Send(new GetUserByIdQuery { Id = id});
        }

        public async Task<UserDTO?> GetUserByLoginAsync(string login)
        {
            return await _mediator.Send(new GetUserByLoginQuery { Login = login });
        }

        public async Task<OperationResultDTO> UpdateUserByIdAsync(Guid id, UserDTO newUserData)
        {
            var userPreliminaryResult = await GetPreliminaryResultAsync(newUserData);
            if (userPreliminaryResult.IsSuccessful)
                await _mediator.Send(new UpdateUserByIdCommand { Id = id, NewUserData = newUserData });
            return userPreliminaryResult;
        }

        private async Task<OperationResultDTO> GetPreliminaryResultAsync(UserDTO user)
        {
            var isSuccessful = true;
            var error = String.Empty;
            var messages = new List<string>();
            var fields = new List<string>();
            if ((await GetUserByLoginAsync(user.Login)) is not null)
            {
                messages.Add(NewsAggregatorMessages.UniqueUserLogin);
                fields.Add(OperationFieldsEnum.Login.ToString());
                error = OperationErrorsEnum.USER_ALREADY_EXISTS.ToString();
                isSuccessful = false;
            }
            if ((await _mediator.Send(new GetUserByEmailQuery { Email = user.Email })) is not null)
            {
                messages.Add(NewsAggregatorMessages.UniqueUserEmail);
                fields.Add(OperationFieldsEnum.Email.ToString());
                error = OperationErrorsEnum.USER_ALREADY_EXISTS.ToString();
                isSuccessful = false;
            }
            return new OperationResultDTO
            {
                IsSuccessful = isSuccessful,
                Error = error,
                Messages = messages,
                Filds = fields
            };
        }

    }
}
