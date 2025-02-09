using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NoteTakingApp.Core.Domains;
using NoteTakingApp.Core.RepositoryContracts;
using NoteTakingApp.Core.ServiceContracts;
using NoteTakingApp.Core.Services;
using NoteTakingApp.Infrastructure.Context;
using NoteTakingApp.Infrastructure.Repositories;
using NoteTakingApp.Middleware;

var builder = WebApplication.CreateBuilder(args);
    
builder.Services.AddControllersWithViews();

// Should be placed after the registration of repositories, services, AppDbContext
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IGetterCountryService, GetterCountryService>();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, AppDbContext, int>>()
    .AddRoleStore<RoleStore<ApplicationRole, AppDbContext, int>>();

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


