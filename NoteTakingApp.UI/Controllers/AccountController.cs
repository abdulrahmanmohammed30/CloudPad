using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Core.Attributes.Enums;
using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Controllers;

[Route("[controller]")]
[AllowAnonymous]
public class AccountController : Controller
{
    private readonly IGetterCountryService _getterCountryService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AccountController(IGetterCountryService getterCountryService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _getterCountryService = getterCountryService;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }


    [HttpGet("register")]
    public async Task<IActionResult> Register()
    {
        ViewBag.Countries = await _getterCountryService.GetAllCountries();
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
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        ViewBag.Countries = await _getterCountryService.GetAllCountries();

        if (!ModelState.IsValid)
        {
            // Todo: Log errors  
            return View(registerDto);
        }

        ApplicationUser user = registerDto.ToEntity();

        // Create the user 
        var userIdentityResult = await _userManager.CreateAsync(user, registerDto.Password);

        if (userIdentityResult.Succeeded == false)
        {
            ModelState.AddModelError("", "Unable to create user");

            foreach (var error in userIdentityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            
            return View(registerDto);
        }

        // The role "User" always exists, Migration folder contains some seeded roles
        // Assign a role to the user 
        var assigningRoleIdentityResult = await _userManager.AddToRoleAsync(user, nameof(UserRole.User));
        if (assigningRoleIdentityResult.Succeeded == false)
        {
            // Todo: Log a warning message 
        }

        return RedirectToAction(nameof(NotesController.Index), "Notes");
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View(new LoginDto());
    }

    // Login 
    //Model is not valid then return view 
    // model is valid then login but could not login return errors to user 
    // if manage to log in then go to notes page 
    
    [HttpPost("login")]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Login(LoginDto loginDto, string? ReturnUrl)
    {
        if (ModelState.IsValid == false)
        {
            // To do: Login data
            return View(loginDto);
        }
        
        var loginResult = await _signInManager.PasswordSignInAsync(loginDto.UserName, loginDto.Password, false, false);
        if (loginResult.Succeeded == false)
        {
            ModelState.AddModelError("", "Invalid username or password");
            return View(loginDto);
        }

        if (!string.IsNullOrEmpty(ReturnUrl)  && Url.IsLocalUrl(ReturnUrl))
            return LocalRedirect(ReturnUrl);

        return RedirectToAction(nameof(NotesController.Index), "Notes");
    }

    [HttpGet("validate-username")]
    public async Task<IActionResult> ValidateUsername(string? username)
    {
        if (username == null)
            return Json(true);

        var user = await _userManager.FindByNameAsync(username);
        return user == null? Json(true): Json(false);
    }
}
