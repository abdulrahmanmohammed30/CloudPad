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
    [CategoryExceptionFilterFactory]
    [Authorize]
    public class CategoryController(ICategoryService categoryService) : Controller
    {
        private int UserId => HttpContext.GetUserId()!.Value;
        [HttpGet("[action]")]
        public async Task<IActionResult> ValidateCategoryName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(true);
            }

            var doesNoteExist = await categoryService.ExistsAsync(UserId, name);
            return Json(!doesNoteExist);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ValidateExistingCategoryName(string name, Guid categoryId)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(true);
            }

            var category = await categoryService.GetByNameAsync(UserId, name);

            if (category == null)
            {
                return Json(true);
            }

            return Json(category.Id == categoryId);
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
                IsFavorite = category.IsFavorite
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
                return View(categoryDto);
            }
            try
            {
                var updatedCategory = await categoryService.UpdateAsync(UserId, categoryDto);
                return RedirectToAction("Index");

            }

            catch (CategoryNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }

            catch (InvalidCategoryException ex)
            {
                // Todo: How to know which field is invalid, Modify the custom exception
                ModelState.AddModelError("", ex.Message);

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View(categoryDto);
            }

            catch (DuplicateCategoryNameException ex)
            {
                // Todo: Does Remote Validation can send the tagName and tag Id or it's just one value? 
                ModelState.AddModelError("Name", ex.Message);
                return View(categoryDto);
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

