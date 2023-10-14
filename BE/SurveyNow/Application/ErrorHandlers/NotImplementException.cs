using System.Net;

namespace Application.ErrorHandlers;

public class NotImplementException : BaseException
{
    private const int _statusCode = (int)HttpStatusCode.NotImplemented;
    private const string? _title = "Method not implement.";

    public NotImplementException()
    {
        StatusCode = _statusCode;
        Title = _title;
    }

    public NotImplementException(string? message) : base(message)
    {
        StatusCode = _statusCode;
        Title = _title;
    }
}