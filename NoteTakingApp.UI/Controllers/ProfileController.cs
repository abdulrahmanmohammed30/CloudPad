using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Core.ServiceContracts;
using NoteTakingApp.Helpers;

namespace NoteTakingApp.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class ProfileController(IUserService userService) : Controller
    {
        public int UserId => HttpContext.GetUserId()!.Value;
        
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var user = await userService.GetUserByIdAsync(UserId);
            return View(user);
        }
    }
}
