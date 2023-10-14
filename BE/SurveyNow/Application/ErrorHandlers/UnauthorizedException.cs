using System.Net;

namespace Application.ErrorHandlers;

public class UnauthorizedException : BaseException
{
    private const int _statusCode = (int)HttpStatusCode.Unauthorized;
    private const string? _title = "Unauthorized.";

    public UnauthorizedException()
    {
        StatusCode = _statusCode;
        Title = _title;
    }

    public UnauthorizedException(string? message) : base(message)
    {
        StatusCode = _statusCode;
        Title = _title;
    }
}