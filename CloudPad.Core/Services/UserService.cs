using System.ComponentModel.DataAnnotations;
using CloudPad.Core.Dtos;
using CloudPad.Core.Entities.Domains;
using CloudPad.Core.Exceptions;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Mappers;
using Microsoft.Extensions.Caching.Memory;

namespace CloudPad.Core.Services
{
    public class UserService(IUserRepository userRepository, IMemoryCache cache) : IUserService
    {
        private readonly MemoryCacheEntryOptions _cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(15))
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));

        private static string GetUserIdCacheKey(int userId) => $"User:Id:{userId}";
        private static string GetUserNameCacheKey(string userName) => $"User:Name:{userName}";

        public async Task<List<UserDto>> GetUsersAsync()
        {
            return (await userRepository.GetUsersAsync()).Select(c => c.ToDto())
                .OrderBy(c => c.Id).ToList();
        }

        public async Task<ProfileDto?> GetUserByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user id");
            }

            ApplicationUser? user;

            if (cache.TryGetValue(GetUserIdCacheKey(userId), out var userObj))
            {
                user = userObj as ApplicationUser;
            }
            else
            {
                user = await userRepository.GetUserByIdAsync(userId);
                if (user != null)
                {
                    cache.Set(GetUserIdCacheKey(user.Id), user, _cacheEntryOptions);
                    cache.Set(GetUserNameCacheKey(user.UserName), user, _cacheEntryOptions);
                }
            }

            return user?.ToProfileDto();
        }

        public async Task<ProfileDto?> GetUserByNameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Invalid username");
            }

            ApplicationUser? user;

            if (cache.TryGetValue(GetUserNameCacheKey(username), out var userObj))
            {
                user = userObj as ApplicationUser;
            }
            else
            {
                user = await userRepository.GetUserByNameAsync(username);
                if (user != null)
                {
                    cache.Set(GetUserIdCacheKey(user.Id), user, _cacheEntryOptions);
                    cache.Set(GetUserNameCacheKey(user.UserName), user, _cacheEntryOptions);
                }
            }

            return user?.ToProfileDto();
        }

        public Task<bool> ExistsAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user id");
            }

            return userRepository.ExistsAsync(userId);
        }

        public async Task<ProfileDto> UpdateProfileAsync(UpdateProfileDto profile)
        {
            var validationContext = new ValidationContext(profile);
            var validationErrors = new List<ValidationResult>();
            if (Validator.TryValidateObject(profile, validationContext, validationErrors) == false)
            {
                throw new ArgumentException(validationErrors.FirstOrDefault()?.ErrorMessage);
            }

            var user = await userRepository.GetUserByIdAsync(profile.Id);
            if (user == null)
            {
                throw new UserNotFoundException($"User with id {profile.Id} was not found");
            }

            user.Name = profile.Name;
            user.Bio = profile.Bio;
            user.PreferredLanguageId = profile.PreferredLanguageId;
            user.BirthDate = profile.BirthDate;
            user.CountryId = profile.CountryId;
            user.Country = null;
            user.PreferredLanguage = null;

            var updatedUser = await userRepository.UpdateAsync(user);

            cache.GetOrCreate(GetUserIdCacheKey(user.Id), entry =>
            {
                entry.SetOptions(_cacheEntryOptions);
                return updatedUser;
            });

            cache.GetOrCreate(GetUserNameCacheKey(user.UserName), entry =>
            {
                entry.SetOptions(_cacheEntryOptions);
                return updatedUser;
            });

            var profileDto = updatedUser.ToProfileDto();
            return profileDto;
        }

        public async Task DeleteUserAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user id");
            }

            var user = await userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException($"User with id {userId} was not found");
            }

            cache.Remove(GetUserIdCacheKey(user.Id));
            cache.Remove(GetUserNameCacheKey(user.UserName));

            await userRepository.DeleteUserAsync(userId);
        }
    }
}