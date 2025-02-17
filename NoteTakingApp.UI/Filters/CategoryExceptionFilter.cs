using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NoteTakingApp.Core.Exceptions;

namespace NoteTakingApp.Filters
{
    public class CategoryExceptionFilterFactoryAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<CategoryExceptionFilter>();
        }
    }

    public class CategoryExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var response = new
            {
                context.Exception.Message
            };

            context.Result = context.Exception switch
            {
                DuplicateCategoryNameException => new BadRequestObjectResult(response),
                InvalidCategoryException => new BadRequestObjectResult(response),
                _ => new ObjectResult(response) { StatusCode = 500 }
            };

            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }
    }
}
