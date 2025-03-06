using CloudPad.Controllers;
using CloudPad.Core.Configurations;
using CloudPad.Core.Domains;
using CloudPad.Core.Entities.Domains;
using CloudPad.Core.RepositoryContracts;
using CloudPad.Core.ServiceContracts;
using CloudPad.Core.Services;
using CloudPad.Filters;
using CloudPad.Infrastructure.Context;
using CloudPad.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Core.Services;

namespace NoteTakingApp.StartupExtensions;

public static class ConfigureServicesExtension
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Should be placed after the registration of repositories, services, AppDbContext
        services.AddMemoryCache();
        services.AddDbContext<AppDbContext>(options =>
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ICountryRetrieverService, CountryRetrieverService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICategoryRetrieverService, CategoryRetrieverService>();
        services.AddScoped<ICategoryManagerService, CategoryManagerService>();
        services.AddScoped<ICategoryValidatorService, CategoryValidatorService>();
        services.AddScoped<INoteFilterService, NoteFilterService>();
        services.AddScoped<INoteManagerService, NoteManagerService>();
        services.AddScoped<INoteRetrieverService, NoteRetrieverService>();
        services.AddScoped<INoteSorterService, NoteSorterService>();
        services.AddScoped<INoteValidatorService, NoteValidatorService>();
        services.AddScoped<ITagManagerService, TagManagerService>();
        services.AddScoped<ITagRetrieverService,TagRetrieverService>();
        services.AddScoped<ITagValidatorService,TagValidatorService>();
        services.AddScoped<IUserValidatorService, UserValidatorService>();
        services.AddScoped<EnsureUserIdExistsFilter>();
        services.AddScoped<IUploadDocumentService, UploadDocumentService>();
        services.AddScoped<IUploadImageService, UploadImageService>();
        services.AddScoped<IUserSocialLinkRepository, UserSocialLinkRepository>();
        services.AddScoped<IUserSocialLinkService, UserSocialLinkService>();
        services.AddScoped<IResourceRepository, ResourceRepository>();
        services.AddScoped<IResourceService, ResourceService>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<ILanguageRetrieverService, LanguageRetrieverService>();
        services.AddTransient<IEmailService, EmailService>();

        services.AddScoped<NoteExceptionFilter>();
        services.AddScoped<CategoryExceptionFilter>();

        services.AddScoped<INoteExcelExportService, NoteExcelExportService>();
        services.AddScoped<INotePdfExportService, NotePdfExportService>();
        services.AddScoped<INoteWordExportService, NoteWordExportService>();

        services.Configure<CountrySettings>(configuration.GetSection("CountrySettings"));
        services.Configure<LanguageSettings>(configuration.GetSection("LanguageSettings"));
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

        services.AddScoped<ResourceExceptionFilter>();
        services.AddScoped<TagExceptionFilter>();
        services.AddScoped<NoteExceptionFilter>();

        services.AddControllersWithViews(options =>
        {
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });


        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/account/login";
        });

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
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

        services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
        });

        services.AddHttpContextAccessor();


        return services;
    }
}