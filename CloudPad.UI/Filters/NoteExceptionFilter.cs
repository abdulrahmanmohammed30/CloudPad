using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using CloudPad.Core.Exceptions;

namespace CloudPad.Filters
{
    public class NoteExceptionFilterFactoryAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
          return serviceProvider.GetRequiredService<NoteExceptionFilter>();
        }
    }

    public class NoteExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var response = new
            {
                 context.Exception.Message
            };
            
            context.Result = context.Exception switch
            {
                UserNotFoundException => new NotFoundObjectResult(response),
                NoteNotFoundException => new NotFoundObjectResult(response),
                CategoryNotFoundException => new NotFoundObjectResult(response),
                TagNotFoundException => new NotFoundObjectResult(response), 
                InvalidCategoryException => new BadRequestObjectResult(response),
                InvalidTagException => new BadRequestObjectResult(response),
                _ => new ObjectResult(response) { StatusCode = (int)HttpStatusCode.InternalServerError }
            };

            context.ExceptionHandled = true;
        }
    }
}
