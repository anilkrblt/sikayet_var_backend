using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                .OrderBy(u => u.Id)
                .ToListAsync();

        public async Task<User> GetUserByIdAsync(int userId, bool trackChanges) =>
            await FindByCondition(u => u.Id == userId, trackChanges).SingleOrDefaultAsync();

        public async Task<User> GetUserByEmailAsync(string email, bool trackChanges) =>
            await FindByCondition(u => u.Email == email, trackChanges).SingleOrDefaultAsync();

        public async Task<User> GetUserByUsernameAsync(string username, bool trackChanges) =>
            await FindByCondition(u => u.Username == username, trackChanges).SingleOrDefaultAsync();

        public void CreateUser(User user) => Create(user);
        public void DeleteUser(User user) => Delete(user);
    }
}
