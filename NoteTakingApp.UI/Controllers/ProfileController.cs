using Microsoft.AspNetCore.Mvc;

namespace NoteTakingApp.Controllers
{
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        [HttpGet("")]
        public IActionResult Get()
        {
            return View();
        }
    }
}
