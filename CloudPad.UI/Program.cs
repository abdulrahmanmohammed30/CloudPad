using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CloudPad.Core.Configurations;
using CloudPad.Core.Domains;
using CloudPad.Core.Entities.Domains;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Services;
using CloudPad.Filters;
using CloudPad.Infrastructure.Context;
using CloudPad.Infrastructure.Repositories;
using CloudPad.Middleware;
using NoteTakingApp.Core.Services;
using PdfSharp.Fonts;
using Rotativa.AspNetCore;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("countries.json", optional: false, reloadOnChange: true)
    .AddJsonFile("languages.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

GlobalFontSettings.UseWindowsFontsUnderWindows = true;

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

// Should be placed after the registration of repositories, services, AppDbContext
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IGetterCountryService, GetterCountryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INoteFilterService, NoteFilterService>();
builder.Services.AddScoped<INoteManagerService, NoteManagerService>();
builder.Services.AddScoped<INoteRetrieverService, NoteRetrieverService>();
builder.Services.AddScoped<INoteSorterService, NoteSorterService>();
builder.Services.AddScoped<INoteValidatorService, NoteValidatorService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IUserValidationService, UserValidationService>();
builder.Services.AddScoped<EnsureUserIdExistsFilter>();
builder.Services.AddScoped<IUploadDocumentService, UploadDocumentService>();
builder.Services.AddScoped<IUploadImageService, UploadImageService>();
builder.Services.AddScoped<IUserSocialLinkRepository, UserSocialLinkRepository>();
builder.Services.AddScoped<IUserSocialLinkService, UserSocialLinkService>();
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<ILanguageRepository, LanguageRepository>();
builder.Services.AddScoped<ILanguageGetterService, LanguageGetterService>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddScoped<NoteExceptionFilter>();
builder.Services.AddScoped<CategoryExceptionFilter>();

builder.Services.AddScoped<INoteExcelExportService, NoteExcelExportService>();
builder.Services.AddScoped<INotePdfExportService, NotePdfExportService>();
builder.Services.AddScoped<INoteWordExportService, NoteWordExportService>();

builder.Services.Configure<CountrySettings>(builder.Configuration.GetSection("CountrySettings"));
builder.Services.Configure<LanguageSettings>(builder.Configuration.GetSection("LanguageSettings"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));


builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
});

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, AppDbContext, int>>()
    .AddRoleStore<RoleStore<ApplicationRole, AppDbContext, int>>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

RotativaConfiguration.Setup(app.Environment.WebRootPath);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseExceptionMiddleware();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

