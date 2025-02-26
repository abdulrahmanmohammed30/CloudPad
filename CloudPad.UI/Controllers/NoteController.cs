using CloudPad.Core.Dtos;
using CloudPad.Core.Exceptions;
using CloudPad.Core.ServiceContracts;
using CloudPad.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CloudPad.Core.Entities.Domains;
using CloudPad.Helpers;
using Rotativa.AspNetCore;

namespace CloudPad.Controllers;

[Route("[controller]")]
[Authorize]
[EnsureUserIdExistsFilterFactory]
public class NoteController(
    INoteRetrieverService noteRetrieverService,
    INoteManagerService noteManagerService,
    ITagService tagService,
    ICategoryService categoryService,
    INoteFilterService noteFilterService,
    INoteExcelExportService excelExportService,
    IWebHostEnvironment webHostEnvironment,
    INoteWordExportService wordExportService
    ) : Controller
{
    private int UserId => HttpContext.GetUserId()!.Value;


    [HttpGet("")]
    public async Task<IActionResult> Index(string tag = "", string title = "", string content = "",
        string category = "", bool isFavorite = false,
        bool isPinned = false, bool isArchived = false, int page = 0, int size = 20)
    {
        var notes = await noteFilterService.FilterAsync(UserId, title, content, tag, category, isFavorite, isPinned,
            isArchived, page, size);
        
        ViewBag.Categories = await categoryService.GetAllAsync(UserId);
        ViewBag.Tags = await tagService.GetAllAsync(UserId);

        return View(notes);
    }

    #region Thinking
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

    #endregion

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var note = await noteRetrieverService.GetByIdAsync(UserId, id);

        if (note == null)
        {
            return NotFound($"Note with id {id} was not found");
        }

        foreach (var resource in note.Resources)
        {
            resource.FilePath = Path.Combine("uploads", resource.FilePath);
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

    [HttpGet("export/excel")]
    public async Task<IActionResult> ExportToExcel()
    {
        var notes = await noteRetrieverService.GetAllAsync(UserId);
        var stream =  excelExportService.GenerateAsync(notes.ToList());
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Notes.xlsx");
        // var fileStream = new FileStream(Path.Combine(webHostEnvironment.WebRootPath, "Meeting in _General_-20240807_105507-Meeting Recording.mp4"), FileMode.Open,
        //     FileAccess.Read);
        // return PhysicalFile(Path.Combine(webHostEnvironment.WebRootPath, "Meeting in _General_-20240807_105507-Meeting Recording.mp4"), "application/mp4");

        // if (System.IO.File.Exists(Path.Combine(webHostEnvironment.WebRootPath,
        //         "Meeting in _General_-20240807_105507-Meeting Recording.mp4")))
        //     return PhysicalFile(
        //         Path.Combine(webHostEnvironment.WebRootPath,
        //             "Meeting in _General_-20240807_105507-Meeting Recording.mp4"),
        //         "application/mp4", "video.mp4", enableRangeProcessing:true);
        // else return Content("File doesn't exist");
    }
    
    [HttpGet("export/pdf")]
    public async Task<IActionResult> ExportToPdf()
    {
        var notes = await noteRetrieverService.GetAllAsync(UserId);
        // var file =   notePdfService.GenerateAsync(notes.ToList());
        // return File(file, "application/pdf", "Notes.pdf");
        return new ViewAsPdf("ExportPdf", notes)
        {
            FileName = "notes.pdf",
            PageSize = Rotativa.AspNetCore.Options.Size.A4
        };
    }
    
    [HttpGet("export/word")]
    public async Task<IActionResult> ExportToWord()
    {
        var notes = await noteRetrieverService.GetAllAsync(UserId);
        var stream =  wordExportService.GenerateAsync(notes.ToList());
        return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Notes.docx");
    }

}