using System.Net;

namespace Application.ErrorHandlers;

public class ConflictException : BaseException
{
    private const int _statusCode = (int)HttpStatusCode.Conflict;
    private const string? _title = "Resource conflict.";

    public ConflictException()
    {
        StatusCode = _statusCode;
        Title = _title;
    }

    public ConflictException(string? message) : base(message)
    {
        StatusCode = _statusCode;
        Title = _title;
    }
}