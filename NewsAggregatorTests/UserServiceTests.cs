using MediatR;
using Moq;
using NewsAggregatorCore;
using NewsAggregatorCore.DTO;
using NewsAggregatorCQS.Commands;
using NewsAggregatorCQS.Querys;
using NewsAggregatorServices.Implementations;

namespace NewsAggregatorTests
{
    public class UserServiceTests
    {
        readonly UserServices _userServices;
        readonly Mock<IMediator> _mediatorMock;

        public UserServiceTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userServices = new UserServices(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_IfUserExists_ReturnUser()
        {
            var random = new Random();
            var userId = Guid.NewGuid();
            var user = new UserDTO
            {
                Id = userId,
                Login = "Logun1",
                Email = "Email1",
                NewsMinRate = random.NextDouble(),
            };
            _mediatorMock.Setup(m =>
                m.Send(It.Is<GetUserByIdQuery>(q => q.Id.Equals(userId)),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            var result = await _userServices.GetUserByIdAsync(userId);
            Assert.NotNull(result);
            Assert.Equal(user.NewsMinRate, result.NewsMinRate);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.Login, result.Login);
            Assert.Equal(user.Email, result.Email);
        }

        [Fact]
        public async Task GetUserByLoginAsync_IfUserExists_ReturnUser()
        {
            var random = new Random();
            var userLogin = "Login1";
            var user = new UserDTO
            {
                Id = Guid.NewGuid(),
                Login = userLogin,
                Email = "Email1",
                NewsMinRate = random.NextDouble(),
            };
            _mediatorMock.Setup(m =>
                m.Send(It.Is<GetUserByLoginQuery>(q => q.Login == userLogin),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            var result = await _userServices.GetUserByLoginAsync(userLogin);
            Assert.NotNull(result);
            Assert.Equal(user.NewsMinRate, result.NewsMinRate);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.Login, result.Login);
            Assert.Equal(user.Email, result.Email);
        }


        [Fact]
        public async Task AddNewUserAsync_IfUserAddded_ReturnPositiveAddUserResult()
        {
            var random = new Random();
            var newUserRole = new RoleDTO 
            { 
                Id = 1,
                RoleName = NewsAggregatorConstants.UserRole 
            };
            var newUser = new UserDTO
            {
                Login = "Login1",
                Email = "Email1",
                NewsMinRate = random.NextDouble(),
                Role = newUserRole
            };
            var positiveAddResult = new AddResourceResultDTO
            {
                Id = Guid.NewGuid(),
                OperationResult = new OperationResultDTO
                {
                    IsSuccessful = true
                }
            };
            _mediatorMock.Setup(m =>
                m.Send(It.Is<GetRoleByNameQuery>(q => q.RoleName == NewsAggregatorConstants.UserRole),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(newUserRole);
            _mediatorMock.Setup(m =>
               m.Send(It.Is<AddUserCommand>(c => c.NewUser.Login == newUser.Login &&
                   c.NewUser.Email == newUser.Email && c.NewUser.NewsMinRate == newUser.NewsMinRate
                   && c.NewUser.Role.RoleName == newUserRole.RoleName),
               It.IsAny<CancellationToken>()))
               .ReturnsAsync(positiveAddResult.Id);
            var result = await _userServices.AddNewUserAsync(newUser);
            Assert.NotNull(result);
            Assert.Equal(positiveAddResult.Id, result.Id);
            Assert.Equal(positiveAddResult.OperationResult.IsSuccessful, result.OperationResult.IsSuccessful);
        }



        [Fact]
        public async Task UpdateUserAsync_IfUserUpdatet_ReturnPositiveOperationResult()
        {
            var userId = Guid.NewGuid();
            var random = new Random();
            var operationResult = new OperationResultDTO
            {
                IsSuccessful = true
            };
            var updateUser = new UpdateUserDTO
            {
                NewEmail = "NewEmail1",
                NewLogin = "NewLogin",
                PasswordHash = "Hash1"
            };
            _mediatorMock.Setup(m =>
              m.Send(It.Is<UpdateUserByIdCommand>(c => c.NewUserData.Email == updateUser.NewEmail &&
                  c.NewUserData.Login == updateUser.NewLogin && c.NewUserData.PasswordHash == updateUser.PasswordHash
                  && c.Id.Equals(userId)),
              It.IsAny<CancellationToken>()));

            var result = await _userServices.UpdateUserByIdAsync(userId, updateUser);
            Assert.NotNull(result);
            Assert.Equal(operationResult.IsSuccessful, result.IsSuccessful);
        }

        [Fact]
        public async Task UpdateUserNewsRateAsync_IfUserDoesNotExists_ThrowsException()
        {
            var random = new Random();
            var userId = Guid.NewGuid();
            var existensUserId = Guid.NewGuid();
            var newNewsRate = random.NextDouble();
            _mediatorMock.Setup(m =>
                m.Send(It.Is<UpdateUserNewsRateByIdCommand>(c => !c.UserId.Equals(existensUserId)),
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            await Assert.ThrowsAsync<Exception>(() => _userServices.UpdateUserNewsRateByIdAsync(userId, newNewsRate));
        }
    }
}
