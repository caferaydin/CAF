using System.Text.Json;
using CAF.Application.Extensions.Authentication;

namespace CAF.WEB.Extensions.Middlewares;

using System.Text.Json;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UserCreateFailedException ex)
        {
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, "Beklenmeyen bir hata oluştu.");
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new { message };
        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}
