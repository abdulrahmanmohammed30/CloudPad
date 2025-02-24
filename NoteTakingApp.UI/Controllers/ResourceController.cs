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
        private int UserId => HttpContext.GetUserId()!.Value;
        private string UserIdentifier=> User.Claims.First(c=>c.Type == "userIdentifier").Value;

        private string UploadedDirectoryPath => Path.Combine(webHostEnvironment.WebRootPath, $"uploads/{UserIdentifier}");
        

        [HttpGet("[action]/{noteId}")]
        public IActionResult Create(Guid noteId)
        {
            return View();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateResource(CreateResourceDto resourceDto)
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }

            try
            {
                var resource = await resourceService.CreateResourceDto(UserId, UploadedDirectoryPath, resourceDto);
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

            var resource = await resourceService.GetByIdAsync(UserId, noteId, resourceId);
            
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
            

            if (!ModelState.IsValid)
            {
                return View(updateResourceDto);
            }

            try
            {
                var updatedResource = await resourceService.UpdateAsync(UserId, noteId, updateResourceDto);
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
           

            var filePath =  Path.Combine(webHostEnvironment.WebRootPath, $"uploads/{resource.FilePath}");
            // remove the resource file from the uploads directory
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return RedirectToAction("Get", "Note", new { id = noteId });

        }
    }
}



