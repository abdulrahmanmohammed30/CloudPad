using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.ServiceContracts;
using NoteTakingApp.Helpers;

namespace NoteTakingApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ResourceController(INoteValidatorService noteValidatorService, IResourceService resourceService, IWebHostEnvironment webHostEnvironment) : Controller
    {
        public int UserId => HttpContext.GetUserId()!.Value;

        [HttpGet("[action]/{noteId}")]
        public async Task<IActionResult> Create(Guid noteId)
        {
           if (!await noteValidatorService.ExistsAsync(UserId, noteId))
            {
                return BadRequest($"Note with id {noteId} doesn't exist");
            }
            return View();
        }

        [HttpPost("[action]")]
       public async Task<IActionResult> CreateResource(CreateResourceDto resourceDto)
            {
            if (!ModelState.IsValid)
            {
                return View(resourceDto);
            }

            if (!await noteValidatorService.ExistsAsync(UserId, resourceDto.NoteId))
            {
                return BadRequest($"Note with id {resourceDto.NoteId} doesn't exist");
            }

            try {
                var uploadedDirectoryPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                var resource = await resourceService.CreateResourceDto(UserId, uploadedDirectoryPath, resourceDto);
                return Json(resource);

            }
            catch(NoteNotFoundException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch(ArgumentNullException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{noteId}")]
        public async Task<IActionResult> GetAllResources(Guid noteId)
        {
            var resources = await resourceService.GetAllResources(noteId);
            return Json(resources);
        }
    }
}
