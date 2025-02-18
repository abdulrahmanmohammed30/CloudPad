using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.ServiceContracts;
using NoteTakingApp.Core.Services;
using NoteTakingApp.Filters;
using NoteTakingApp.Helpers;

namespace NoteTakingApp.Controllers
{
    [Route("[controller]")]
    [EnsureUserIdExistsFilterFactory]
    [CategoryExceptionFilterFactory]
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
            return View(categories);
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

                var categoryDto = await categoryService.CreateAsync(UserId, createCategoryDto);
                return Json(categoryDto);
        }

        // What options 
        // Id could be empty, return bad request invalid category id 
        // otherwise, check if the note category exists 
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Update(Guid id)
        {
            if(!await categoryService.ExistsAsync(UserId, id))
            {
                return BadRequest($"Category with id {id} doesn't exist");
            }

            var category = await categoryService.GetByIdAsync(UserId, id);
            
            UpdateCategoryDto updateCategoryDto = new UpdateCategoryDto()
            {
                CategoryId = category.Id,
                Name = category.Name,
                Description = category.Description,
            };

            return View(updateCategoryDto);
        }

        [HttpPost("[action]/{categoryId}")]
        public async Task<IActionResult> Update([FromRoute] Guid categoryId, UpdateCategoryDto categoryDto)
        {
            if (ModelState.IsValid == false)
            {
                return View(categoryId);
            }
            var updatedCategory = await categoryService.UpdateAsync(UserId, categoryDto);
            return Json(updatedCategory);
        }
    }
}

