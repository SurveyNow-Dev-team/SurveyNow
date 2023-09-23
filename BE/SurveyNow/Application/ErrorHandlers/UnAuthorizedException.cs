using System.Net;

namespace Application.ErrorHandlers;

public class UnAuthorizedException : BaseException
{
    public UnAuthorizedException()
    {
        StatusCode = (int)HttpStatusCode.Unauthorized;
        Title = "Unauthorized.";
    }

    public UnAuthorizedException(string? message) : base(message)
    {
        StatusCode = (int)HttpStatusCode.Unauthorized;
        Title = "Unauthorized.";
    }
}