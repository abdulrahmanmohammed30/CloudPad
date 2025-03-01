using System.Linq.Expressions;
using CloudPad.Core.Dtos;
using CloudPad.Core.Exceptions;
using CloudPad.Core.ServiceContracts;
using CloudPad.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CloudPad.Helpers;
using DocumentFormat.OpenXml.InkML;
using NoteTakingApp.Model_Binders;

namespace CloudPad.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [TagExceptionFilterFactory]
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
        public async Task<IActionResult> ValidateExistingTagName(string name, int id)
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

            return Json(tag.Id == id);
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

        [HttpGet("create-tag")]
        public IActionResult CreateTag([ModelBinder(BinderType=typeof(CreateTagDtoModelBinderV2))] CreateTagDto tag)
        {
                return Json(tag);
        }
        
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateTagDto createTagDto)
        {
            if (ModelState.IsValid == false)
            {
                return View(createTagDto);
            }

            await tagService.CreateAsync(UserId, createTagDto);
            return RedirectToAction("Index");
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


            var updatedTag = await tagService.UpdateAsync(UserId, updateTagDto);
            return RedirectToAction("Index");
        }


        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Tag Id is not valid");
            }

            await tagService.DeleteAsync(UserId, id);
            return RedirectToAction("Index");
        }
    }
}