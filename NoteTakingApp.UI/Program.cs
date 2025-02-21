using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Core.Configurations;
using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;
using NoteTakingApp.Core.Services;
using NoteTakingApp.Filters;
using NoteTakingApp.Infrastructure.Context;
using NoteTakingApp.Infrastructure.Repositories;
using NoteTakingApp.Middleware;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddScoped<IResourceRepository, ResourceRepository>();

builder.Services.AddScoped<IResourceService, ResourceService>();

builder.Services.AddScoped<NoteExceptionFilter>();
builder.Services.AddScoped<CategoryExceptionFilter>();


builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddTransient<IEmailService, EmailService>();

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

var app = builder.Build();

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


