using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public UserService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(bool trackChanges)
        {
            var users = await _repository.User.GetAllUsersAsync(trackChanges);
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            return usersDto;
        }

        public async Task<UserDto> GetUserByIdAsync(int userId, bool trackChanges)
        {
            var user = await _repository.User.GetUserByIdAsync(userId, trackChanges);
            if (user is null)
            {
                _logger.LogInfo($"User with id: {userId} not found in the database.");
                return null;
            }
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

       
        public async Task<UserDto> GetUserByEmailAsync(string email, bool trackChanges)
        {
            var user = await _repository.User.GetUserByEmailAsync(email, trackChanges);
            if (user is null)
            {
                _logger.LogInfo($"User with email: {email} not found in the database.");
                return null;
            }
            return _mapper.Map<UserDto>(user);
        }

        
        public async Task<UserDto> GetUserByUsernameAsync(string username, bool trackChanges)
        {
            var user = await _repository.User.GetUserByUsernameAsync(username, trackChanges);
            if (user is null)
            {
                _logger.LogInfo($"User with username: {username} not found in the database.");
                return null;
            }
            return _mapper.Map<UserDto>(user);
        }

       
        public async Task<UserDto> CreateUserAsync(UserDto userDto)
        {
            var userEntity = _mapper.Map<User>(userDto);
            _repository.User.CreateUser(userEntity);
            await _repository.SaveAsync();
            var createdUser = _mapper.Map<UserDto>(userEntity);
            return createdUser;
        }

      
        public async Task DeleteUserAsync(int userId)
        {
            var user = await _repository.User.GetUserByIdAsync(userId, trackChanges: false);
            if (user is null)
            {
                _logger.LogInfo($"User with id: {userId} not found in the database.");
                return;
            }
            _repository.User.DeleteUser(user);
            await _repository.SaveAsync();
        }
    }
}
