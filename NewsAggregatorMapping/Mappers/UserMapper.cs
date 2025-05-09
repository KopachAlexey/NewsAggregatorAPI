using NewsAggregatorCore.DTO;
using NewsAggregatorData.Entities;
using NewsAggregatorModels.Models;
using Riok.Mapperly.Abstractions;

namespace NewsAggregatorMapping.Mappers
{
    [Mapper]
    public partial class UserMapper
    {
        [MapperIgnoreTarget(nameof(User.Id))]
        [MapperIgnoreTarget(nameof(User.Role))]
        public partial User UserDTOToEntity(UserDTO userDTO);
        public partial UserDTO EntityToUserDTO(User user);

        [MapperIgnoreSource(nameof(AddUserRequest.Password))]
        public partial UserDTO NewUserReuestToUserDTO(AddUserRequest userRequest);

        [MapProperty(nameof(UserDTO.Role.RoleName), nameof(UserResponse.RoleName))]
        public partial UserResponse UserDTOToUserResponse(UserDTO userDTO);
    }
}
