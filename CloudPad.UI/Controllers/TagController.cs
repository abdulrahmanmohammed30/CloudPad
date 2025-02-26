using CloudPad.Core.Dtos;
using CloudPad.Core.Exceptions;
using CloudPad.Core.ServiceContracts;
using CloudPad.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CloudPad.Helpers;

namespace CloudPad.Controllers
{
    [Route("[controller]")]
    [EnsureUserIdExistsFilterFactory]
    [Authorize]
    public class TagController(ITagService tagService) : Controller
    {
        private int UserId => HttpContext.GetUserId()!.Value;

        [HttpGet("[action]")]
        public async Task<IActionResult> ValidateTagName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(true);
            }

            var doesTagWithSameNameExist = await tagService.ExistsAsync(UserId, name);
            return Json(!doesTagWithSameNameExist);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ValidateExistingTagName(string name, int tagId)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(true);
            }

            var tag = await tagService.GetByNameAsync(UserId, name);

            if (tag == null)
            {
                return Json(true);
            }

            return Json(tag.Id == tagId);
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {

            var tags = await tagService.GetAllAsync(UserId);
            return View(tags);
        }

        [HttpGet("[action]")]
        public IActionResult Create()
        {
            return View(new CreateTagDto());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateTagDto createTagDto)
        {
            if (ModelState.IsValid == false)
            {
                return View(createTagDto);
            }

            try
            {
                var tagDto = await tagService.CreateAsync(UserId, createTagDto);
                return RedirectToAction("Index");
            }
            catch (DuplicateTagNameException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Tag Id is not valid");
            }

            var tag = await tagService.GetByIdAsync(UserId, id);

            if (tag == null)
            {
                return NotFound($"Tag with id {id} was not found");
            }

            var tagDto = new UpdateTagDto()
            {
                TagId = tag.Id,
                Name = tag.Name,
                Description = tag.Description
            };

            return View(tagDto);
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Update(int id, UpdateTagDto updateTagDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateTagDto);
            }

            if (id <= 0)
            {
                return BadRequest("tag id is not valid");
            }

            try
            {
                var updatedTag = await tagService.UpdateAsync(UserId, updateTagDto);
                return RedirectToAction("Index");
            }

            catch (TagNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }

            catch (DuplicateTagNameException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View(updateTagDto);
            }
        }



        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Tag Id is not valid");
            }

            try
            {
                await tagService.DeleteAsync(UserId, id);
                return RedirectToAction("Index");
            }
            catch (TagNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
