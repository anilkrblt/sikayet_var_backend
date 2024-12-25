using System.Security.Cryptography;
using System.Text;
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

        public async Task RegisterUserAsync(UserForRegistrationDto userDto)
        {
            var userrDto = new UserDto(8,userDto.Email,userDto.Password,userDto.Username, "user");
            if (userDto is null)
            {
                _logger.LogError("RegisterUserAsync: UserForRegistrationDto object is null.");
                throw new ArgumentNullException(nameof(userDto));
            }

            var userEntity = _mapper.Map<User>(userDto);

                var existingUserByEmail = await _repository.User.GetUserByEmailAsync(userDto.Email, trackChanges: false);
            if (existingUserByEmail != null)
            {
                _logger.LogError($"RegisterUserAsync: User with email {userDto.Email} already exists.");
                throw new InvalidOperationException($"User with email {userDto.Email} already exists.");
            }

            var existingUserByUsername = await _repository.User.GetUserByUsernameAsync(userDto.Username, trackChanges: false);
            if (existingUserByUsername != null)
            {
                _logger.LogError($"RegisterUserAsync: User with username {userDto.Username} already exists.");
                throw new InvalidOperationException($"User with username {userDto.Username} already exists.");
            }

            userEntity.PasswordHash = HashPassword(userDto.Password);

            try
            {
                _repository.User.CreateUser(userEntity);
                await _repository.SaveAsync();
                _logger.LogInfo($"RegisterUserAsync: User with id {userEntity.Id} registered successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"RegisterUserAsync: An error occurred while saving the user - {ex.Message}");
                throw;
            }
        }



        public async Task UpdateUserAsync(int userId, UserForUpdateDto userDto)
        {
            if (userDto is null)
            {
                _logger.LogError("UpdateUserAsync: UserForUpdateDto object is null.");
                throw new ArgumentNullException(nameof(userDto));
            }

            // Retrieve the user from the database
            var userEntity = await _repository.User.GetUserByIdAsync(userId, trackChanges: true);
            if (userEntity is null)
            {
                _logger.LogError($"UpdateUserAsync: User with id {userId} not found.");
                throw new KeyNotFoundException($"User with id {userId} not found.");
            }

            // Update user fields
            _mapper.Map(userDto, userEntity);

            // Save changes
            await _repository.SaveAsync();

            _logger.LogInfo($"UpdateUserAsync: User with id {userId} updated successfully.");
        }



        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }


    }
}
