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
            Message = GetMessage(exception),
            Error = exception.Message
        };

        // var jsonResponse = JsonConvert.SerializeObject(response);
        return context.Response.WriteAsync(response.ToString());
    }

    private static int GetStatusCode(Exception exception)
    {
        // Customize status codes based on exception types
        if (exception is NotFoundException)
        {
            return (int)HttpStatusCode.NotFound;
        }
        else if (exception is BadRequestException)
        {
            return (int)HttpStatusCode.BadRequest;
        }
        else
        {
            return (int)HttpStatusCode.InternalServerError;
        }
    }

    private static string GetMessage(Exception exception)
    {
        // Customize error messages based on exception types
        if (exception is NotFoundException)
        {
            return "Resource not found.";
        }
        else if (exception is BadRequestException)
        {
            return "Bad request.";
        }
        else
        {
            return "Internal server error.";
        }
    }
}