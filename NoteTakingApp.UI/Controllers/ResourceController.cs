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

            try
            {
                var uploadedDirectoryPath = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                var resource = await resourceService.CreateResourceDto(UserId, uploadedDirectoryPath, resourceDto);
                return RedirectToAction("Get", "Note", new { id = resourceDto.NoteId });
            }
            catch (NoteNotFoundException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("[action]/{noteId}/{resourceId}")]
        public async Task<IActionResult> Update(Guid noteId, Guid resourceId)
        {
            if (noteId == Guid.Empty)
            {
                return BadRequest("node id is invalid");
            }

            if(resourceId == Guid.Empty)
            {
                return BadRequest("resourceId is invalid");
            }

            var resource = await resourceService.GetByIdAsync(resourceId);
            
            if(resource == null)
            {
                return NotFound($"Resource with id {resourceId} was not found");
            }

            var updateResourceDto = new UpdateResourceDto()
            {
                ResourceId = resource.ResourceId,
                Description = resource.Description,
                DisplayName = resource.DisplayName,
                FilePath = resource.FilePath,
                NoteId = noteId,
                Size = resource.Size
            };

            return View(updateResourceDto);
        }

        [HttpPost("[action]/{noteId}/{resourceId}")]
        public async Task<IActionResult> Update(Guid noteId, Guid resourceId, UpdateResourceDto updateResourceDto) {
        
            if (noteId == Guid.Empty)
            {
                return BadRequest("node id is invalid");
            }

            if(resourceId == Guid.Empty)
            {
                return BadRequest("resourceId is invalid");
            }

            if(updateResourceDto == null)
            {
                return BadRequest("resource is null");
            }

            if (!ModelState.IsValid)
            {
                return View(updateResourceDto);
            }

            try
            {
                var updatedResource = await resourceService.UpdateAsync(UserId, updateResourceDto);
                return RedirectToAction("Get", "Note", new { id = noteId });
            }
            catch (ResourceNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidResourceException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(updateResourceDto);
            }
        }


        [HttpGet("{noteId}")]
        public async Task<IActionResult> GetAllResources(Guid noteId)
        {
            var resources = await resourceService.GetAllResources(noteId);
            return Json(resources);
        }

        [HttpPost("[action]/{noteId}/{resourceId}")]
        public async Task<IActionResult> Delete(Guid noteId, Guid resourceId)
        {
            if (noteId == Guid.Empty)
            {
                return BadRequest("node id is invalid");
            }

            if (resourceId == Guid.Empty)
            {
                return BadRequest(new { message = "resource id is invalid" });
            }

            var resource = await resourceService.GetByIdAsync(resourceId);

            if (resource == null)
            {
                return NotFound(new { message = $"Resource with id {resourceId} was not found" });
            }

            if (!await resourceService.DeleteAsync(resourceId))
            {
                return BadRequest($"Failed to delete resource with id {resourceId}");
            }

            // remove the resource file from the uploads directory
            if (System.IO.File.Exists(resource.FilePath))
            {
                System.IO.File.Delete(resource.FilePath);
            }

            return RedirectToAction("Get", "Note", new { id = noteId });

        }
    }
}



