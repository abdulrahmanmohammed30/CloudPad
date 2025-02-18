using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.ServiceContracts;
using NoteTakingApp.Filters;
using NoteTakingApp.Helpers;

namespace NoteTakingApp.Controllers
{
    [Route("[controller]")]
    [EnsureUserIdExistsFilterFactory]
    [Authorize]
    public class TagController(ITagService tagService) : Controller
    {   
        private int UserId => HttpContext.GetUserId()!.Value;

        public async Task<IActionResult> ValidateTagName(string name)
        {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
            var doesTagWithSameNameExist = await tagService.ExistsAsync(UserId, name);
            return Json(!doesTagWithSameNameExist);
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
                return Json(tagDto
                        );
            }
            catch (DuplicateTagNameException ex) { 
                return BadRequest(new {error = ex.Message});
            }
        }
    }
}
