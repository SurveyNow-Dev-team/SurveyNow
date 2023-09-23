using System.Net;

namespace Application.ErrorHandlers;

public class ConflictException : BaseException
{
    public ConflictException()
    {
        StatusCode = (int)HttpStatusCode.Conflict;
        Title = "Resource conflict.";
    }

    public ConflictException(string? message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.Conflict;
        Title = "Resource conflict.";
    }
}