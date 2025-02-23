using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Core.Attributes.Enums;
using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.Entities.Domains;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Controllers;

[Authorize(Roles = "Admin")]
[Route("[controller]/users")]
public class AdminController(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IUserService userService)
    : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> GetAllUsers(string? errorMessage, int page = 1, int pageSize = 10)
    {
        var usersWithRoles = await userService.GetUsersAsync();
        ViewBag.errorMessage = errorMessage;
        ViewBag.pageNumber = page;
        ViewBag.pageSize = pageSize;
        return View(usersWithRoles);
    }

    /*// attempt to maintain the same page*/

    [HttpGet("assign-role")]
    public async Task<IActionResult> AssignRole(string? username, string? role, int page = 1, int pageSize = 10)
    {
        if (username == null || role == null)
        {
            return RedirectToAction(nameof(GetAllUsers), "Admin",
                new { page, pageSize, errorMessage = "User and role are required" });
        }

        var user = await userManager.FindByNameAsync(username);
        if (user == null)
        {
            return RedirectToAction(nameof(GetAllUsers), "Admin",
                new { page, pageSize, errorMessage = "User was not found" });
        }
        
        if (await roleManager.RoleExistsAsync(role) == false)
        {
            return RedirectToAction(nameof(GetAllUsers), "Admin",
                new { page, pageSize, errorMessage = "role was not found" });

        }
        // uawe can only have one role but that's not how Identity architecture works 
        // so we will enforce that, by removing all roles user has which is at most can be one 
        // I don't know if a role doesn't exist would it count it as error or not
        var removeRolesResult=await userManager.RemoveFromRolesAsync(user, Enum.GetNames(typeof(UserRole)));

        var res = await userManager.AddToRoleAsync(user, role);
        
        return res.Succeeded == false
            ? RedirectToAction(nameof(GetAllUsers), "Admin",
                new { page, pageSize, errorMessage = "Failed to assign a role to user" })
            : RedirectToAction(nameof(GetAllUsers), "Admin", new { page, pageSize });
    }

    // attempt to maintain the same page 
    [HttpGet("delete")]
    public async Task<IActionResult> DeleteUser(string? username, int? page = 1, int? pageSize = 10)
    {
        if (username == null)
        {
            return RedirectToAction(nameof(GetAllUsers), "Admin",
                new { page, pageSize, errorMessage = "User id is required" });
        }

        var user = await userManager.FindByNameAsync(username);
        if (user == null)
        {
            return RedirectToAction(nameof(GetAllUsers), "Admin",
                new { page, pageSize, errorMessage = "User was not found" });
        }

        var res = await userManager.DeleteAsync(user);

        return res.Succeeded == false
            ? RedirectToAction(nameof(GetAllUsers), "Admin",
                new { page, pageSize, errorMessage = "Failed to delete user" })
            : RedirectToAction(nameof(GetAllUsers), "Admin", new { page, pageSize });
    }
}