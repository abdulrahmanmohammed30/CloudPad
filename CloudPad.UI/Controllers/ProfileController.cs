using CloudPad.Core.Dtos;
using CloudPad.Core.Entities.Domains;
using CloudPad.Core.Exceptions;
using CloudPad.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CloudPad.Helpers;

namespace CloudPad.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class ProfileController(
        IUserService userService,
        IGetterCountryService countryService,
        ILanguageGetterService languageGetterService,
        IUserSocialLinkService userSocialLinkService,
        IUploadImageService uploadImageService,
        IWebHostEnvironment webHostEnvironment,
        UserManager<ApplicationUser> userManager) : Controller
    {
        public int UserId => HttpContext.GetUserId()!.Value;
        private string UserIdentifier=> User.Claims.First(c=>c.Type == "userIdentifier").Value;
        private string UploadsDirectoryPath => Path.Combine(webHostEnvironment.WebRootPath, $"uploads/{UserIdentifier}");

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            
            var user = await userService.GetUserByIdAsync(UserId);

            if (user == null)
            {
                return NotFound("failed to load profile data");
            }
            
            if (string.IsNullOrEmpty(user.ProfileImageUrl) == false)
                user.ProfileImageUrl = Path.Combine("uploads", user.ProfileImageUrl);
            return View(user);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Update()
        {
            var user = await userService.GetUserByIdAsync(UserId);
            
            if (user == null)
            {
                return NotFound("failed to load profile data");
            }
            
            var updateProfileDto = new UpdateProfileDto()
            {
                Id = user.Id,
                CountryId = user.CountryId,
                Name = user.Name,
                PreferredLanguageId = user.PreferredLanguageId,
                Bio = user.Bio,
                BirthDate = user.BirthDate
            };

            ViewBag.Countries = await countryService.GetAllCountriesAsync();
            ViewBag.Languages = await languageGetterService.GetAllAsync();
            return View(updateProfileDto);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update(UpdateProfileDto updateProfileDto)
        {
            ViewBag.Countries = await countryService.GetAllCountriesAsync();
            ViewBag.Languages = await languageGetterService.GetAllAsync();

            if (!ModelState.IsValid)
            {
                return View(updateProfileDto);
            }

            try
            {
                await userService.UpdateProfileAsync(updateProfileDto);
                return RedirectToAction("Index");
            }
            catch (UserNotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(updateProfileDto);
            }
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> AddLink(CreateUserSocialLinkDto createUserSocialLinkDto)
        {
            if (ModelState.IsValid == false)
            {
                TempData["Error"] = ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage;
            }
            else
            {
                try
                {
                    await userSocialLinkService.CreateAsync(createUserSocialLinkDto);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> DeleteLink(int id)
        {
            if (ModelState.IsValid == false)
            {
                TempData["Error"] = "id is invalid";
            }
            else
            {
                if (await userSocialLinkService.DeleteAsync(UserId, id) == false)
                {
                    TempData["Error"] = "Failed to delete link";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangeProfileImage(IFormFile? profileImage)
        {
            if (profileImage == null || profileImage.Length == 0)
            {
                TempData["Error"] = "file is empty";
            }
            else
            {
                try
                {
                    var profileImageUrl =
                        await uploadImageService.UploadAsync(UploadsDirectoryPath, profileImage);

                    var user = await userManager.FindByIdAsync(UserId.ToString());
                    var oldProfileImageUrl = Path.Combine(UploadsDirectoryPath, user.ProfileImageUrl);

                    user.ProfileImageUrl = profileImageUrl;

                    var identityResult = await userManager.UpdateAsync(user);

                    if (identityResult.Succeeded == false)
                    {
                        TempData["Error"] = identityResult.Errors.FirstOrDefault()?.Description;
                        if (!string.IsNullOrEmpty(profileImageUrl) && System.IO.File.Exists(profileImageUrl))
                        {
                            System.IO.File.Delete(profileImageUrl);
                        }
                    }
                    else if (!string.IsNullOrEmpty(oldProfileImageUrl) && System.IO.File.Exists(Path.Combine(webHostEnvironment.WebRootPath,oldProfileImageUrl)))
                    {
                        System.IO.File.Delete(Path.Combine(webHostEnvironment.WebRootPath,oldProfileImageUrl));
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }
            return RedirectToAction("Index");
        }
    }
}