using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Core.Dtos;
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

            try 
                var uploadedDirectoryPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                var resource = await resourceService.CreateResourceDto(UserId, uploadedDirectoryPath, resourceDto);
                return RedirectToAction("Get", "Note", new {id=resourceDto.NoteId});

            }
            catch(NoteNotFoundException ex)
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

        [HttpPost("[action]/{resourceId}")]
        public async Task<IActionResult> Delete(Guid resourceId)
        {
            if (resourceId == Guid.Empty)
            {
                return BadRequest(new { message = "resource id is invalid" });
            }

           try {
               if (await resourceService.DeleteAsync(resourceId))
                {
                    return BadRequest($"Failed to delete resource with id {resourceId}");
                }
               return RedirectToAction("Index");
            }

            catch(ResourceNotFoundException ex) {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
