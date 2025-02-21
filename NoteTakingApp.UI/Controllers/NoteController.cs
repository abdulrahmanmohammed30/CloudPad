using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.ServiceContracts;
using NoteTakingApp.Filters;
using NoteTakingApp.Helpers;

namespace NoteTakingApp.Controllers;

[Route("[controller]")]
[Authorize]
[EnsureUserIdExistsFilterFactory]
public class NoteController(INoteRetrieverService noteRetrieverService, INoteManagerService noteManagerService,
  ITagService tagService, ICategoryService categoryService, INoteFilterService noteFilterService) : Controller
{
    private int UserId => HttpContext.GetUserId()!.Value;


    [HttpGet("")]
    public async Task<IActionResult> Index(string tag = "", string title = "", string content = "", string category = "", bool isFavorite = false,
        bool isPinned = false, bool isArchived = false, int page = 0, int size = 20)
    {
        var notes = await noteFilterService.FilterAsync(UserId, title, content, tag, category, isFavorite, isPinned, isArchived, page, size);

        HttpContext.Response.Cookies.Append("preferredLanguage", "en-US", new CookieOptions()
        {
            Expires = DateTime.Now.AddDays(1),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });
        ViewBag.Categories = await categoryService.GetAllAsync(UserId);
        ViewBag.Tags = await tagService.GetAllAsync(UserId);

        return View(notes);
    }
    // all user categories should be cached: select, cache categories 
    // all user tags should be cached: multi-select, cache tags 
    // ways to handle boolean types 
    // handle exceptions 
    // ctr + q 
    // do all the checks necessary to avoid the exceptions 
    // surround the code with try-catch 
    // catch for different exceptions 
    // we may use Postman for postsing requests and after creating all the controller route, 
    // we will create the views 
    // create and update are very similar 
    // test using index to get all notes 
    // return createdNote 

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var note = await noteRetrieverService.GetByIdAsync(UserId, id);

        if (note == null)
        {
            return NotFound($"Note with id {id} was not found");
        }

        HttpContext.Response.Cookies.Append("preferredLanguage", "en-US", new CookieOptions()
        {
            Expires = DateTime.Now.AddDays(1),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        return View(note);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await categoryService.GetAllAsync(UserId);
        ViewBag.Tags = await tagService.GetAllAsync(UserId);
        return View(new CreateNoteDto());
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Create(CreateNoteDto noteDto)
    {
        if (ModelState.IsValid == false)
        {
            // Categories and Tags are cached on the server 
            ViewBag.Categories = await categoryService.GetAllAsync(UserId);
            ViewBag.Tags = await tagService.GetAllAsync(UserId);
            return View(noteDto);
        }

        noteDto.Tags = noteDto.Tags == null ? [] : noteDto.Tags;
        var note = await noteManagerService.AddAsync(UserId, noteDto);
        return RedirectToAction("Index");

    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> Update(Guid id)
    {
        var existingNote = await noteRetrieverService.GetByIdAsync(UserId, id);

        if (existingNote == null)
        {
            return NotFound($"Note with id {id} was not found");
        }

        var note = new UpdateNoteDto()
        {
            NoteId = existingNote.Id,
            Title = existingNote.Title,
            Content = existingNote.Content,
            Tags = existingNote.Tags.Select(t => t.Id).ToList(),
            CategoryId = existingNote.Category?.Id,
            IsArchived = existingNote.IsArchived,
            IsFavorite = existingNote.IsFavorite,
            IsPinned = existingNote.IsPinned,
        };

        ViewBag.Categories = await categoryService.GetAllAsync(UserId);
        ViewBag.Tags = await tagService.GetAllAsync(UserId);

        return View(note);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateNote(UpdateNoteDto noteDto)
    {

        if (ModelState.IsValid == false)
        {
            // Categories and Tags are cached on the server 
            ViewBag.Categories = await categoryService.GetAllAsync(UserId);
            ViewBag.Tags = await tagService.GetAllAsync(UserId);
            return View(noteDto);
        }

        try
        {
            noteDto.Tags = noteDto.Tags == null ? [] : noteDto.Tags;
            var updatedNote = await noteManagerService.UpdateAsync(UserId, noteDto);
            return RedirectToAction("Index");
        }

        catch (NoteNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPost("[action]/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (await noteManagerService.DeleteAsync(UserId, id) == false)
            {
                return BadRequest(new { message = $"Failed to delete note with id {id}" });
            }
            return RedirectToAction("Index");
        }
        catch (NoteNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}