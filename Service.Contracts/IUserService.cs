using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync(bool trackChanges);
        Task<UserDto> GetUserByIdAsync(int userId, bool trackChanges);
        Task<UserDto> GetUserByEmailAsync(string email, bool trackChanges);
        Task<UserDto> GetUserByUsernameAsync(string username, bool trackChanges);
        Task<UserDto> CreateUserAsync(UserDto userDto);
        Task DeleteUserAsync(int userId);
        Task RegisterUserAsync(UserForRegistrationDto userDto);
        Task UpdateUserAsync(int userId, UserForUpdateDto userDto);

    }
}
