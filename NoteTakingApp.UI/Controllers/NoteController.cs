using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NoteTakingApp.Core.Dtos;
using NoteTakingApp.Core.Exceptions;
using NoteTakingApp.Core.Mappers;
using NoteTakingApp.Core.ServiceContracts;
using NoteTakingApp.Filters;
using NoteTakingApp.Helpers;
using Xunit.Sdk;

namespace NoteTakingApp.Controllers;

[Route("[controller]")]
[Authorize]
[EnsureUserIdExistsFilterFactory]
public class NoteController: Controller
{
    private readonly INoteRetrieverService noteRetrieverService;
    private readonly INoteManagerService noteManagerService;
    private readonly ITagService tagService;
    private readonly ICategoryService categoryService;
    private int UserId => HttpContext.GetUserId()!.Value;

    public NoteController(INoteRetrieverService noteRetrieverService,
    INoteManagerService noteManagerService,
    ITagService tagService,
    ICategoryService categoryService)
    {
        this.noteRetrieverService = noteRetrieverService;
        this.noteManagerService = noteManagerService;
        this.tagService = tagService;
        this.categoryService = categoryService;
    }


    [HttpGet("")]
    public async Task<IActionResult> Index([FromBody]int page=0, [FromBody] int size=20)
    { 
        var notes = await noteRetrieverService.GetAllAsync(UserId, page, size);
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

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var note = await noteRetrieverService.GetByIdAsync(UserId, id);

        if (note == null)
        {
            return NotFound($"Note with id {id} was not found");
        }
        
        return Json(note);
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

        try
        {
            noteDto.Tags = noteDto.Tags == null ? [] : noteDto.Tags; 
            var note=await noteManagerService.AddAsync(UserId, noteDto);
            return RedirectToAction("Index");
        }
        catch (TagNotFoundException ex)
        {
            return BadRequest(new { error = "Some of the tags assigned to the note could not be found" });
        }
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
            return NotFound(new { error =$"Note with id {noteDto.NoteId} was not found." });
        }
        
        catch (TagNotFoundException ex)
        {
            return BadRequest(new { message = $"Some of the tags assigned to not with id {noteDto.NoteId} were not be found" });
        }
    }

    [HttpGet("[action]/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (await noteManagerService.DeleteAsync(UserId, id) == false)
            {
                return BadRequest(new { Rmessage = $"Failed to delete note with id {id}" });
            }
            return RedirectToAction("Index");
        }
        catch (NoteNotFoundException ex)
        {
            return NotFound(new { message = $"Note with id: {id} was not found" });
        }
    }
    
    
}