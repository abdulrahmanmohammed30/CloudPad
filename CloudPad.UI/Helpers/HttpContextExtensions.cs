using System.Security.Claims;

namespace CloudPad.Helpers;

public static class HttpContextExtensions
{
    /// <summary>
    /// Cast the userId to int
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns>Returns the userId value</returns>
    /// <exception cref="KeyNotFoundException">Throws a KeyNotFoundException If the userId was not found</exception>
    public static int? GetUserId(this HttpContext httpContext)
    {
        var userIdStr = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(userIdStr, out int id) ? id : null;
    }
}
