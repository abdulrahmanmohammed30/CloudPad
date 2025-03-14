﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudPad.Controllers;

[AllowAnonymous]
public class HomeController: Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        
        return View();
    }

    [Route("/error")]
    public IActionResult Error()
    {
        return View();
    }
}

