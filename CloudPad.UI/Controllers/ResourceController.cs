using CloudPad.Core.Dtos;
using CloudPad.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CloudPad.Helpers;

namespace CloudPad.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ResourceExceptionFilterAttributeFactory]
    public class ResourceController(
        INoteValidatorService noteValidatorService,
        IResourceService resourceService,
        IWebHostEnvironment webHostEnvironment) : Controller
    {
        private int UserId => HttpContext.GetUserId()!.Value;
        private string UserIdentifier => User.Claims.First(c => c.Type == "userIdentifier").Value;

        private string UploadedDirectoryPath =>
            Path.Combine(webHostEnvironment.WebRootPath, $"uploads/{UserIdentifier}");


        [HttpGet("[action]/{noteId}")]
        public async Task<IActionResult> Create(Guid noteId)
        {
            if (noteId == Guid.Empty)
            {
                return BadRequest("Invalid note id exception");
            }

            if (await noteValidatorService.ExistsAsync(UserId, noteId))
            {
                return NotFound($"Note with id {noteId} was not found");
            }

            return View(new CreateResourceDto() { NoteId = noteId });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateResource(CreateResourceDto resourceDto)
        {
            if (resourceDto.NoteId == Guid.Empty)
            {
                throw new InvalidResourceException("Note id is required");
            }
            
            if (!ModelState.IsValid)
            {
                return View("Create");
            }

 
                await resourceService.CreateAsync(UserId, UploadedDirectoryPath, resourceDto);
                return RedirectToAction("Get", "Note", new { id = resourceDto.NoteId });

        }

        [HttpGet("[action]/{noteId}/{resourceId}")]
        public async Task<IActionResult> Update(Guid noteId, Guid resourceId)
        {
            if (noteId == Guid.Empty)
            {
                return BadRequest("node id is invalid");
            }

            if (resourceId == Guid.Empty)
            {
                return BadRequest("resourceId is invalid");
            }

            var resource = await resourceService.GetByIdAsync(UserId, noteId, resourceId);

            if (resource == null)
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
        public async Task<IActionResult> Update(Guid noteId, Guid resourceId, UpdateResourceDto updateResourceDto)
        {
            if (noteId == Guid.Empty)
            {
                return BadRequest("node id is invalid");
            }

            if (resourceId == Guid.Empty)
            {
                return BadRequest("resourceId is invalid");
            }


            if (!ModelState.IsValid)
            {
                return View(updateResourceDto);
            }


               await resourceService.UpdateAsync(UserId, noteId, updateResourceDto);
                return RedirectToAction("Get", "Note", new { id = noteId });
   
        }


        [HttpGet("{noteId}")]
        public async Task<IActionResult> GetAllResources(Guid noteId)
        {  
            if (noteId == Guid.Empty)
            {
                return BadRequest("node id is invalid");
            }
            
            var resources = await resourceService.GetAllResources(UserId, noteId);
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

            var resource = await resourceService.GetByIdAsync(UserId, noteId, resourceId);

            if (resource == null)
            {
                return NotFound(new { message = $"Resource with id {resourceId} was not found" });
            }

            await resourceService.DeleteAsync(UserId, noteId, resourceId);


            var filePath = Path.Combine(webHostEnvironment.WebRootPath, $"uploads/{resource.FilePath}");
            // remove the resource file from the uploads directory
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return RedirectToAction("Get", "Note", new { id = noteId });
        }
    }
}