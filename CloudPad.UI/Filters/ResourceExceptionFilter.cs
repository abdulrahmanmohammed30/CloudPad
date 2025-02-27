using System.Net;
using CloudPad.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CloudPad.Controllers;

public class ResourceExceptionFilterAttributeFactory : Attribute, IFilterFactory
{
    public bool IsReusable { get; } = false;
    
    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredService<ResourceExceptionFilter>();
    }
}

public class ResourceExceptionFilter:IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var response = new
        {
            context.Exception.Message
        };

        context.Result = context.Exception switch
        {
            NoteNotFoundException  => new NotFoundObjectResult(response),
            ResourceNotFoundException  => new NotFoundObjectResult(response),

            _ => new ObjectResult(response) { StatusCode = (int)HttpStatusCode.InternalServerError }
        };
        context.ExceptionHandled=true;
    }
}