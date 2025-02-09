using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Controllers;

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


    [HttpGet("/register")]
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
    [HttpPost("/register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        ViewBag.Countries = await _getterCountryService.GetAllCountries();

        if (!ModelState.IsValid)
        {
            // Todo: Log errors  
            return View(registerDto);
        }

        // Assume user did not use the website form to post the data but instead,
        // typed any invalid id, and posted the data using any tool, basically bypassing the form validation
        if (registerDto.CountryId.HasValue && (
                registerDto.CountryId <= 0 ||
                !await _getterCountryService.Exists((short)registerDto.CountryId)))
        {
            return BadRequest("Invalid country Id");
        }

        //Same, What if a malicious user typed posted a username that's in use
        if (await _userManager.FindByNameAsync(registerDto.UserName.Trim()) != null)
        {
            return BadRequest("Username is already taken");
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

        // Create the role 
        // Check first whether the role exists or not 
        // If it doesn't exist then creat it and if it was created successfully then add the role to the user otherwise,
        // log a message informing that an error has occured and user was not assigned a role
        // If the role already exists, then assign it to the user 
        //  _userManager.AddToRoleAsync(user, role);

        string role = registerDto.UserRole.ToString();
        if (!await _roleManager.RoleExistsAsync(role))
        {
            var roleIdentityResult = await _roleManager.CreateAsync(new ApplicationRole() { Name = role });
            if (roleIdentityResult.Succeeded)
                await _userManager.AddToRoleAsync(user, role);
        }
        else
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        return RedirectToAction(nameof(NotesController.Index), "Notes");
    }

    [HttpGet("/login")]
    public IActionResult Login()
    {
        return View(new LoginDto());
    }

    // Login 
    //Model is not valid then return view 
    // model is valid then login but could not login return errors to user 
    // if manage to log in then go to notes page 
    
    [HttpPost("/login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
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
        
        return RedirectToAction(nameof(NotesController.Index), "Notes");
    }
}
