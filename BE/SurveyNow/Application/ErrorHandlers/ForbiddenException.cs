using System.Net;

namespace Application.ErrorHandlers;

public class ForbiddenException : BaseException
{
    private const int _statusCode = (int)HttpStatusCode.Forbidden;
    private const string? _title = "Access to resource is forbidden.";

    public ForbiddenException()
    {
        StatusCode = _statusCode;
        Title = _title;
    }

    public ForbiddenException(string? message) : base(message)
    {
        StatusCode = _statusCode;
        Title = _title;
    }
}