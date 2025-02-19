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
            return RedirectToAction("Index");
        }

        // What options 
        // Id could be empty, return bad request invalid category id 
        // otherwise, check if the note category exists 
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Update(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new { message = "Category id is invalid" });
            }

            var category = await categoryService.GetByIdAsync(UserId, id);

            if (category == null)
            {
                return NotFound($"category was id {id} was not found");
            }

            UpdateCategoryDto updateCategoryDto = new UpdateCategoryDto()
            {
                CategoryId = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsFavorite=category.IsFavorite
            };

            return View(updateCategoryDto);
        }

        [HttpPost("[action]/{categoryId}")]
        public async Task<IActionResult> Update([FromRoute] Guid categoryId, UpdateCategoryDto categoryDto)
        {
            if (categoryId == Guid.Empty)
            {
                return BadRequest(new { message = "Category id is invalid" });
            }
            
            if (categoryDto == null)
            {
                return BadRequest(new { message = "Category is null" });
            }

            if (ModelState.IsValid == false)
            {
                return View(categoryId);
            }
            try
            {
            var updatedCategory = await categoryService.UpdateAsync(UserId, categoryDto);
            return RedirectToAction("Index");

            }

            catch (InvalidCategoryException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            catch (CategoryNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }

            catch (DuplicateCategoryNameException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }


        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new { message = "Category id is invalid" });
            }

            try
            {
                if (await categoryService.DeleteAsync(UserId, id) == false)
                {
                    return BadRequest(new { Rmessage = $"Failed to delete category with id {id}" });
                }
                return RedirectToAction("Index");
            }
            catch (CategoryNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}

