using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CloudPad.Core.Mappers;
using System.Security.Claims;
using CloudPad.Core.Attributes.Enums;
using CloudPad.Core.Dtos;
using CloudPad.Core.Entities.Domains;
using CloudPad.Core.ServiceContracts;
using CloudPad.Helpers;

namespace CloudPad.Controllers;

[Route("[controller]")]
[AllowAnonymous]
public class AccountController(
    ICountryRetrieverService countryRetrieverService,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IEmailService emailService,
    IWebHostEnvironment webHostEnvironment,
    IUploadImageService uploadImageService,
    IUserService userService
    )
    : Controller
{
    private string UserIdentifier=> User.Claims.First(c=>c.Type == "userIdentifier").Value;

    private string UploadsDirectoryPath => Path.Combine(webHostEnvironment.WebRootPath, $"uploads/{UserIdentifier}");

    [HttpGet("")]
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }


    [HttpGet("register")]
    public async Task<IActionResult> Register()
    {
        if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Note");
        }

        ViewBag.Countries = await countryRetrieverService.GetAllCountriesAsync();
        return View(new RegisterDto());
    }

    // What logical steps? 
    // ModelState is invalid -> return 
    // Map to User Entity 
    // Trim all the strings when mapping to user 
    // use the usermanager to add the user 
    // use the signinmanager to sign in the user and create cookie 
    // user the userroles to add a role to the user 
    // at each step validate that work so for is correct 
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        ViewBag.Countries = await countryRetrieverService.GetAllCountriesAsync();
        if (!ModelState.IsValid)
        {
            // Todo: Log errors  
            return View(registerDto);
        }

        ApplicationUser user = registerDto.ToEntity();
        

        // Create the user 
        var userIdentityResult = await userManager.CreateAsync(user, registerDto.Password);

        if (userIdentityResult.Succeeded == false)
        {
            ModelState.AddModelError("", "Unable to create user");

            foreach (var error in userIdentityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(registerDto);
        }

        var userIdentifier = Guid.NewGuid().ToString();
        var uploadsDirectoryPath = Path.Combine(webHostEnvironment.WebRootPath, $"uploads/{userIdentifier}");
        await userManager.AddClaimAsync(user, new Claim("userIdentifier", userIdentifier ));
        
        Directory.CreateDirectory(uploadsDirectoryPath);

        try
        {
            if (registerDto.ImageFile != null && registerDto.ImageFile.Length != 0)
            {
                user.ProfileImageUrl =
                    await uploadImageService.UploadAsync(uploadsDirectoryPath,
                        registerDto.ImageFile);
            }
        }
        catch (Exception ex)
        {
            // ModelState.AddModelError("", ex.Message);
            // return View(registerDto);
            // notify the user 
        }

        // generate email confirmation token 
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Scheme);
        
        await emailService.SendEmailAsync(new EmailRequest()
        {
            To = user.Email!,
            Subject = "Confirm your email",
            Body = $"Please confirm your email by clicking {confirmationLink}"   
        });

        // The role "User" always exists, Migration folder contains some seeded roles
        // Assign a role to the user 
        var assigningRoleIdentityResult = await userManager.AddToRoleAsync(user, nameof(UserRole.User));
        if (assigningRoleIdentityResult.Succeeded == false)
        {
            // Todo: Log a warning message 
        }

        
        ViewBag.userEmail = user.Email;
        return View("RegistrationSuccessful");

        //return RedirectToAction(nameof(NoteController.Index), "Note");
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Note");
        }

        return View(new LoginDto());
    }

    // Login 
    //Model is not valid then return view 
    // model is valid then login but could not login return errors to user 
    // if manage to log in then go to notes page 

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto, string? returnUrl)
    {
        if (ModelState.IsValid == false)
        {
            // To do: Login data
            return View(loginDto);
        }

        var user = await userManager.FindByNameAsync(loginDto.UserName);

        if (user == null)
        {
            ViewBag.Errors = new List<string> { "Invalid username or password" };
            return View(loginDto);
        }
        
        if (user.EmailConfirmed == false)
        {
            ModelState.AddModelError(string.Empty, "You must confirm your email before logging in.");
            ViewBag.EmailNotConfirmed = true;
            return View(loginDto);
        }

        var loginResult = await signInManager.PasswordSignInAsync(loginDto.UserName, loginDto.Password, false, false);

        if (loginResult.Succeeded == false)
        {
            ModelState.AddModelError("", "Invalid username or password");
            return View(loginDto);
        }
        
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return LocalRedirect(returnUrl);

        return RedirectToAction(nameof(NoteController.Index), "Note");
    }

    [HttpPost("signout")]
    [Authorize]
    public async Task<IActionResult> Signout()
    {
        await signInManager.SignOutAsync();

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpGet("validate-username")]
    public async Task<IActionResult> ValidateUsername(string? username)
    {
        if (username == null)
            return Json(true);

        var user = await userManager.FindByNameAsync(username);
        return user == null ? Json(true) : Json(false);
    }

    [HttpGet("send-email")]
    public IActionResult SendEmail()
    {
        return View(new EmailRequest());
    }


    [HttpGet("confirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
        {
            return BadRequest("Invalid email confirmation request");
        }

        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        var result = await userManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded)
        {   
            return View("EmailConfirmed"); // Success message
        }

        return BadRequest("Email confirmation failed.");
    }

    [HttpGet("forget-password")]
    public IActionResult ForgetPassword()
    {
        return View(new ForgetPasswordDto());
    }

    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordDto rorgetPasswordDto)
    {
        if (ModelState.IsValid == false)
        {
            return View(rorgetPasswordDto);
        }

        var user = await userManager.FindByEmailAsync(rorgetPasswordDto.Email);

        if (user == null)
        {
            return NotFound($"User with email {rorgetPasswordDto.Email} was not found.");
        }

        if ((await userManager.IsEmailConfirmedAsync(user)) == false)
        {
            ModelState.AddModelError(string.Empty, "You must confirm your email before logging in.");
            ViewBag.EmailNotConfirmed = true;
            return View(rorgetPasswordDto);
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var confirmationLink = Url.Action("ResetPassword", "Account", new { userId = user.Id, token }, Request.Scheme);

        await emailService.SendEmailAsync(new EmailRequest()
        {
            To = user.Email!,
            Subject = "Confirm your email",
            Body = $"Please confirm your email by clicking {confirmationLink}"   
        });

        ViewBag.Email = rorgetPasswordDto.Email;
        return View("ForgetPasswordConfirmation");
    }

    [HttpGet("reset-password")]
    public async Task<IActionResult> ResetPassword(string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
        {
            return BadRequest("Invalid password reset request");
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"Unable to load user.");
        }

        var isValidToken = await userManager.VerifyUserTokenAsync(user,
            userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);

        if (isValidToken == false)
        {
            return BadRequest("Invalid password reset request");
        }

        var resetPasswordDto = new ResetPasswordDto()
        {
            UserId = userId,
            Token = token
        };
        return View(resetPasswordDto);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        if (ModelState.IsValid == false)
        {
            return View(resetPasswordDto);
        }

        var user = await userManager.FindByIdAsync(resetPasswordDto.UserId);
        if (user == null)
        {
            return NotFound($"User with id {resetPasswordDto.UserId} was not found.");
        }

        var result = await userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);

        if (result.Succeeded == false)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(resetPasswordDto);
        }

        return View("ResetPasswordConfirmation");
    }

    [HttpGet("[action]")]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View(new ChangePasswordDto());
    }

    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        if (ModelState.IsValid == false)
        {
            return View(changePasswordDto);
        }

        var user = await userManager.FindByIdAsync(HttpContext.GetUserId()!.Value.ToString());
        if (user == null)
        {
            return NotFound($"Unable to load user.");
        }

        var result =
            await userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

        if (result.Succeeded == false)
        {
            TempData["Error"] = "Unable to change password";
        }

        return RedirectToAction("Index");
    }

    [HttpGet("[action]")]
    [Authorize]
    public IActionResult ChangeEmail()
    {
        return View(new UpdateEmailDto());
    }

    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> ChangeEmail(UpdateEmailDto updateEmailDto)
    {
        if (ModelState.IsValid == false)
        {
            return View(updateEmailDto);
        }

        var user = await userManager.FindByIdAsync(HttpContext.GetUserId()!.Value.ToString());
        if (user == null)
        {
            return NotFound($"Unable to load user.");
        }

        var token = await userManager.GenerateChangeEmailTokenAsync(user, updateEmailDto.Email);
        var confirmChangeToken = Url.Action("ConfirmEmailChange", "Account",
            new { UserId = user.Id, updateEmailDto.Email, token }, Request.Scheme);

        await emailService.SendEmailAsync(new EmailRequest()
        {
            To = user.Email!,
            Subject = "Confirm your email",
            Body = $"Please confirm your email by clicking {confirmChangeToken}"   
        });

        ViewBag.userEmail = user.Email;

        TempData["Notification"] = "Email change request has been sent to your email address.";

        return RedirectToAction("Index");
    }

    [HttpGet("[action]")]
    [Authorize]
    public async Task<IActionResult> ConfirmEmailChange(ConfirmEmailChangeDto confirmEmailChangeDto)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest("Invalid data");
        }

        var user = await userManager.FindByIdAsync(HttpContext.GetUserId()!.Value.ToString());
        if (user == null)
        {
            return NotFound($"Unable to load user.");
        }

        var result = await userManager.ChangeEmailAsync(user, confirmEmailChangeDto.Email, confirmEmailChangeDto.Token);
        if (result.Succeeded == false)
        {
            return BadRequest("Email confirmation failed.");
        }

        TempData["Notification"] = "Email has been updated successfully.";

        return RedirectToAction("Index");
    }

    [HttpGet("[action]")]
    [Authorize]
    public async Task<IActionResult> DeleteAccount()
    {
        try
        {
            var user = await userManager.FindByIdAsync(HttpContext.GetUserId()!.Value.ToString());
            if (user == null)
            {
                return NotFound($"Unable to load user.");
            }

            await userService.DeleteUserAsync(user.Id);
            if (Directory.Exists(UploadsDirectoryPath))
            {
                Directory.Delete(UploadsDirectoryPath, true);
            }
            // await tagService.DeleteAllAsync(user.Id);
            // var result = await userManager.DeleteAsync(user);
            // if (result.Succeeded == false)
            // {
            //     throw new InvalidOperationException("Unexpected error occurred deleting user.");
            // }

            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Unable to delete account";
            return RedirectToAction("Index");
        }
    }
}