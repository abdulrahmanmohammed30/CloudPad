﻿namespace CloudPad.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // log the error message 
            if (ex.InnerException != null)
            {
                Console.WriteLine(ex.InnerException.GetType().ToString());
                Console.WriteLine(ex.InnerException.Message);
            }
            else
            {
                Console.WriteLine(ex.GetType().ToString());
                Console.WriteLine(ex.Message);
            }

            throw;
        }
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
} 