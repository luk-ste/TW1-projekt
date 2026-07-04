
using EventManagement.BL.Exceptions;
using EventManagement.BL.Interfaces;
using EventManagement.BL.Security;
using EventManagement.DAL.Models;
using EventManagement.DAL.Repositories;
using Microsoft.Extensions.Configuration;

namespace EventManagement.BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IConfiguration _configuration;

        public UserService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUserRoleRepository userRoleRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _configuration = configuration;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<string> RegisterAsync(
            string username, string password,
            string firstName, string lastName,
            string email, string? phone)
        {
            // Check username not already taken
            var existing = await _userRepository.GetByUsernameAsync(username.Trim());
            if (existing != null)
                throw new BadRequestException($"Username {username.Trim()} already exists");

            // Hash password
            var salt = PasswordHashProvider.GetSalt();
            var hash = PasswordHashProvider.GetHash(password, salt);

            var user = new User
            {
                Username = username.Trim(),
                PasswordHash = hash,
                PasswordSalt = salt,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone
            };

            await _userRepository.AddAsync(user);
            await AssignDefaultRoleAsync(user.Id);

            return user.Username;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var genericLoginFail = "Incorrect username or password";

            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                throw new UnauthorizedException(genericLoginFail);

            var hash = PasswordHashProvider.GetHash(password, user.PasswordSalt);
            if (hash != user.PasswordHash)
                throw new UnauthorizedException(genericLoginFail);

            var role = user.UserRoles.FirstOrDefault()?.Role?.Name ?? "User";
            var secureKey = _configuration["JWT:SecureKey"];
            return JwtTokenProvider.CreateToken(secureKey, 120, user.Username, role);
        }

        public async Task ChangePasswordAsync(string username, string oldPassword, string newPassword)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                throw new NotFoundException("User not found");


            var oldHash = PasswordHashProvider.GetHash(oldPassword, user.PasswordSalt);
            if (oldHash != user.PasswordHash)
                throw new BadRequestException("Old password is incorrect");

            var newSalt = PasswordHashProvider.GetSalt();
            var newHash = PasswordHashProvider.GetHash(newPassword, newSalt);

            user.PasswordHash = newHash;
            user.PasswordSalt = newSalt;

            await _userRepository.UpdateAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"User with id {id} not found");

            await _userRepository.DeleteAsync(id);
        }

        public async Task AssignDefaultRoleAsync(int userId)
        {
            var userRole = await _roleRepository.GetByNameAsync("User");
            if (userRole == null)
                throw new NotFoundException("Default role 'User' not found");

            await _userRoleRepository.AssignRoleAsync(new UserRole
            {
                UserId = userId,
                RoleId = userRole.Id
            });
        }

        public async Task PromoteToAdminAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"User with id {id} not found");

            var adminRole = await _roleRepository.GetByNameAsync("Admin");
            if (adminRole == null)
                throw new NotFoundException("Admin role not found");

            await _userRoleRepository.AssignRoleAsync(new UserRole
            {
                UserId = id,
                RoleId = adminRole.Id
            });
        }
    }
}