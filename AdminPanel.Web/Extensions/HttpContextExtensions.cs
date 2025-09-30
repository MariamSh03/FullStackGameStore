namespace AdminPanel.Web.Extensions;

public static class HttpContextExtensions
{
    public static string? GetAcceptLanguage(this HttpContext httpContext)
    {
        return httpContext.Request.Headers.AcceptLanguage.FirstOrDefault();
    }

    public static string? GetAcceptLanguage(this HttpRequest request)
    {
        return request.Headers.AcceptLanguage.FirstOrDefault();
    }
}