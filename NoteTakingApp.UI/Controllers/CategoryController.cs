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
    public class CategoryController(ICategoryService categoryService) : Controller
    {
        private int UserId => HttpContext.GetUserId()!.Value;
        [HttpGet("[action]")]
        public async Task<IActionResult> ValidateCategoryName(string name)
        {
            var doesNoteExist = await categoryService.ExistsAsync(UserId, name);
            return Json(!doesNoteExist);
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var categories = await categoryService.GetAllAsync(UserId);
            return Json(categories);
        }

        [HttpGet("[action]")]
        public IActionResult Create()
        {
            return View(new CreateCategoryDto());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateCategoryDto createCategoryDto)
        {
            if (ModelState.IsValid == false)    
            {
                return View(createCategoryDto);
            }

            try
            {
                var categoryDto = await categoryService.CreateAsync(UserId, createCategoryDto);
                return Json(categoryDto);
            }
            catch (DuplicateCategoryNameException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
