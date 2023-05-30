namespace Ticketing.Auth.Application.Extensions;

public static class CookieExtension
{
    public static void SetCookie(
        this IHttpContextAccessor httpContextAccessor,
        string key,
        string value,
        int expireInDays = 7)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(expireInDays),
            SameSite = SameSiteMode.Strict,
            Secure = true
        };

        httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, cookieOptions);
    }
    
    public static void RemoveCookie(this IHttpContextAccessor httpContextAccessor, string key)
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
    }
}