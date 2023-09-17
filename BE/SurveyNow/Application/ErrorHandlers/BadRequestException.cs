using System.Net;

namespace Application.ErrorHandlers;

public class BadRequestException: BaseException
{
    public BadRequestException()
    {
        StatusCode = (int)HttpStatusCode.BadRequest;
        Title = "Bad Request.";
    }

    public BadRequestException(string? message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.BadRequest;
        Title = "Bad Request.";
    }
}