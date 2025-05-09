﻿using CloudPad.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CloudPad.Filters
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
                Message= context.Exception.InnerException != null? context.Exception.InnerException.Message:context.Exception.Message
            };

            context.Result = context.Exception switch
            {
                CategoryNotFoundException => new NotFoundObjectResult(response),
                DuplicateCategoryNameException => new BadRequestObjectResult(response),
                InvalidCategoryException => new BadRequestObjectResult(response),
                _ => new ObjectResult(response) { StatusCode = 500 }
            };

            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }
    }
}
