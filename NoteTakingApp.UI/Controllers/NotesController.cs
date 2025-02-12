using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Core.ServiceContracts;

namespace NoteTakingApp.Controllers;

[Route("notes")]
public class NotesController(INoteRetrieverService noteRetrieverService) : Controller
{
    private readonly INoteRetrieverService _noteRetrieverService = noteRetrieverService;

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdStr, out int userId))
        {
            return Json(new { message = "UserId is invalid" });
        }

        var notes = await _noteRetrieverService.GetAllAsync(userId);
        return Json(notes);
    }
}
