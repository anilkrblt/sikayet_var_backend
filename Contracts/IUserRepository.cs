using Entities.Models;

namespace Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges);
        Task<User> GetUserByIdAsync(int userId, bool trackChanges);
        Task<User> GetUserByEmailAsync(string email, bool trackChanges);
        Task<User> GetUserByUsernameAsync(string username, bool trackChanges);
        void CreateUser(User user);
        void DeleteUser(User user);
    }
}
