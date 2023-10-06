using System.Net;

namespace Application.ErrorHandlers;

public class ForbiddenException : BaseException
{
    public ForbiddenException()
    {
        StatusCode = (int)HttpStatusCode.Forbidden;
        Title = "Access to resource is forbidden.";
    }

    public ForbiddenException(string? message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.Forbidden;
        Title = "Access to resource is forbidden.";
    }
}