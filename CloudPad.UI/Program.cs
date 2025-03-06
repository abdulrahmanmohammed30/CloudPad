using CloudPad.Middleware;
using NoteTakingApp.StartupExtensions;
using PdfSharp.Fonts;
using Rotativa.AspNetCore;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("countries.json", optional: false, reloadOnChange: true)
    .AddJsonFile("languages.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

GlobalFontSettings.UseWindowsFontsUnderWindows = true;

builder.Services.ConfigureServices(builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

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

app.Use(async (context, next) =>
{
    var routeValues = context.Request.RouteValues;

    if (routeValues.TryGetValue("pageNumber", out var pageNumberObj)
        && pageNumberObj is int pageNumber
        && pageNumber <= 0)
    {
        routeValues["pageNumber"] = 1;
    }

    if (routeValues.TryGetValue("pageSize", out var pageSizeObj)
        && pageSizeObj is int pageSize
        && pageSize <= 0)
    {
        routeValues["pageSize"] = 20;
    }

    await next.Invoke();
});
app.Run();