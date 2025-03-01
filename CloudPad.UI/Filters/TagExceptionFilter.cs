using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using CloudPad.Core.Exceptions;

namespace CloudPad.Filters
{
    public class TagExceptionFilterFactoryAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
          return serviceProvider.GetRequiredService<TagExceptionFilter>();
        }
    }

    public class TagExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var response = new
            {
                Message= context.Exception.InnerException != null? context.Exception.InnerException.Message:context.Exception.Message
            };
            
            context.Result = context.Exception switch
            {
                TagNotFoundException => new NotFoundObjectResult(response),
                DuplicateTagNameException => new BadRequestObjectResult(response),
                InvalidNoteIdException=> new BadRequestObjectResult(response),
                _ => new ObjectResult(response) { StatusCode = (int)HttpStatusCode.InternalServerError }
            };

            context.ExceptionHandled = true;
        }
    }
}
