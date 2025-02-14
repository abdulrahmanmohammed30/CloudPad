using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NoteTakingApp.Controllers;

//[Route("")]
[AllowAnonymous]
public class HomeController: Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        return Json(HttpContext.GetEndpoint()?.DisplayName);
        //return View();
    }

    [Route("/error")]
    public IActionResult Error()
    {
        return View();
    }
}

