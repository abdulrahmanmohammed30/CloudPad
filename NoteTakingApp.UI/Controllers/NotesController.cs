using Microsoft.AspNetCore.Mvc;

namespace NoteTakingApp.Controllers;

[Route("notes")]
public class NotesController : Controller
{
    // GET
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }
}
