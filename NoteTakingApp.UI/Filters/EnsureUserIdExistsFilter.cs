using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NoteTakingApp.Helpers;

namespace NoteTakingApp.Filters;

public class EnsureUserIdExistsFilterFactoryAttribute: Attribute, IFilterFactory
{
    public bool IsReusable { get; } = false;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var instance = serviceProvider.GetRequiredService<EnsureUserIdExistsFilter>();
        return instance;
    }
}

public class EnsureUserIdExistsFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.GetUserId() == null)
        {
            context.Result = new BadRequestObjectResult(new { message = "UserId is invalid" });
        }

        await next();
    }
}