using System.Net;

namespace Application.ErrorHandlers;

public class NotFoundException : BaseException
{
    private const int _statusCode = (int)HttpStatusCode.NotFound;
    private const string? _title = "Resource not found.";

    public NotFoundException()
    {
        StatusCode = _statusCode;
        Title = _title;
    }

    public NotFoundException(string? message) : base(message)
    {
        StatusCode = _statusCode;
        Title = _title;
    }
}