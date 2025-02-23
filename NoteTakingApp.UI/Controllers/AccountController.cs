using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using NoteTakingApp.Core.Attributes.Enums;
using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.ServiceContracts;
using System.Net;
using System.Text;
using NoteTakingApp.Core.Entities.Domains;

namespace NoteTakingApp.Controllers;

[Route("[controller]")]
[AllowAnonymous]
public class AccountController(
    IGetterCountryService getterCountryService,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    RoleManager<ApplicationRole> roleManager,
    IEmailService emailService,
    IUploadDocumentService uploadDocumentService,
    IWebHostEnvironment webHostEnvironment,
    IUploadImageService uploadImageService)
    : Controller
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;


    [HttpGet("register")]
    public async Task<IActionResult> Register()
    {
        if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Note");
        }

        ViewBag.Countries = await getterCountryService.GetAllCountries();
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
        ViewBag.Countries = await getterCountryService.GetAllCountries();

        if (!ModelState.IsValid)
        {
            // Todo: Log errors  
            return View(registerDto);
        }

        ApplicationUser user = registerDto.ToEntity();

    try
        {
            if (registerDto.ImageFile != null && registerDto.ImageFile.Length != 0)
            {
                user.ProfileImageUrl = await uploadImageService.Upload(Path.Combine(webHostEnvironment.WebRootPath, "uploads"), registerDto.ImageFile);
            }
        }catch(Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(registerDto);
        }

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


        // generate email confirmation token 
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token))
            .Replace("+", "-")  // Replace + with -
            .Replace("/", "_")  // Replace / with _
            .TrimEnd('=');      // Remove padding
        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        // var confirmationLink = $"{baseUrl}/account/confirmemail?userId={user.Id}&token={encodedToken}";

        var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token }, Request.Scheme);



        await emailService.SendEmailAsync(user.Email!, "Confirm Your Email", $"Please confirm your email by clicking {confirmationLink}.");

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
    public async Task<IActionResult> Login(LoginDto loginDto, string? ReturnUrl)
    {
        if (ModelState.IsValid == false)
        {
            // To do: Login data
            return View(loginDto);
        }

        var user = await userManager.FindByNameAsync(loginDto.UserName);

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

        if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
            return LocalRedirect(ReturnUrl);

        return RedirectToAction(nameof(NoteController.Index), "Note");
    }

    [HttpPost("signout")]
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
        var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(
            token.Replace("-", "+").Replace("_", "/") + new string('=', (4 - token.Length % 4) % 4)));
        if (userId == null || token == null)
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

        await emailService.SendEmailAsync(rorgetPasswordDto.Email, "Reset your account pasword", $"Please confirm your email by clicking {confirmationLink}");

        ViewBag.Email = rorgetPasswordDto.Email;
        return View("ForgetPasswordConfirmation");
    }

    [HttpGet("reset-password")]
    public IActionResult ResetPassword(string userId, string token)
    {
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
}


