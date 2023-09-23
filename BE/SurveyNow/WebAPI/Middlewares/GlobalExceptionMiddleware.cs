using System.Net;
using Application.ErrorHandlers;

namespace SurveyNow.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = GetStatusCode(exception);

        var response = new ErrorDetail()
        {
            StatusCode = context.Response.StatusCode,
            Title = GetTitle(exception),
            Message = exception.Message
        };

        // var jsonResponse = JsonConvert.SerializeObject(response);
        return context.Response.WriteAsync(response.ToString());
    }

    private static int GetStatusCode(Exception exception)
    {
        // Customize status codes based on exception types
        if (exception is BaseException baseException)
        {
            return baseException.StatusCode;
        }
        else
        {
            return (int)HttpStatusCode.InternalServerError;
        }
    }

    private static string GetTitle(Exception exception)
    {
        // Customize error messages based on exception types
        if (exception is BaseException baseException)
        {
            return baseException?.Title ?? "";
        }
        else
        {
            return "Internal server error.";
        }
    }
}