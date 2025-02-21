using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NoteTakingApp.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class ProfileController : Controller
    {
        [HttpGet("")]
        public IActionResult Get()
        {
            return View();
        }
    }
}
