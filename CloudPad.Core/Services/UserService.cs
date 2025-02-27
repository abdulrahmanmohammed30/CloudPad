using System.ComponentModel.DataAnnotations;
using CloudPad.Core.Dtos;
using CloudPad.Core.Exceptions;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Mappers;

namespace CloudPad.Core.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<List<UserDto>> GetUsersAsync()
        {
            return (await userRepository.GetUsersAsync()).Select(c => c.ToDto()).OrderBy(c => c.Id).ToList();
        }

        public async Task<ProfileDto?> GetUserByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user id");
            }
            
            return (await userRepository.GetUserByIdAsync(userId))?.ToProfileDto();
        }

        public async Task<ProfileDto?> GetUserByNameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Invalid username");
            }
            return (await userRepository.GetUserByNameAsync(username))?.ToProfileDto();
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

            var profileDto = (await userRepository.UpdateAsync(user)).ToProfileDto();
            return profileDto;
        }

        public async Task DeleteUserAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("Invalid user id");
            }
            
             await userRepository.DeleteUserAsync(userId);
        }

    }
}