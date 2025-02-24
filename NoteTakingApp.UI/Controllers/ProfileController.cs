using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities.Domains;
using NoteTakingApp.Core.ServiceContracts;
using NoteTakingApp.Helpers;

namespace NoteTakingApp.Controllers
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

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var user = await userService.GetUserByIdAsync(UserId);
            return View(user);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Update()
        {
            var user = await userService.GetUserByIdAsync(UserId);
            var updateProfileDto = new UpdateProfileDto()
            {
                Id = user.Id,
                CountryId = user.CountryId,
                Name = user.Name,
                PreferredLanguageId = user.PreferredLanguageId,
                Bio = user.Bio,
                BirthDate = user.BirthDate
            };

            ViewBag.Countries = await countryService.GetAllCountries();
            ViewBag.Languages = await languageGetterService.GetAllAsync();
            return View(updateProfileDto);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Update(UpdateProfileDto updateProfileDto)
        {
            ViewBag.Countries = await countryService.GetAllCountries();
            ViewBag.Languages = await languageGetterService.GetAllAsync();

            if (!ModelState.IsValid)
            {
                return View(updateProfileDto);
            }

            try
            {
                var user = await userService.UpdateProfileAsync(updateProfileDto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
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
                if (await userSocialLinkService.DeleteAsync(id) == false)
                {
                    TempData["Error"] = "Failed to delete link";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangeProfileImage(IFormFile? ProfileImage)
        {
            ProfileImage = null;
            
            if (ProfileImage == null || ProfileImage.Length == 0)
            {
                TempData["Error"] = "file is empty";
            }
            else
            {
                try
                {
                    var profileImageUrl =
                        await uploadImageService.Upload(Path.Combine(webHostEnvironment.WebRootPath, "uploads"), ProfileImage);

                    var user = await userManager.FindByIdAsync(UserId.ToString());
                    var oldProfileImageUrl = user.ProfileImageUrl;

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