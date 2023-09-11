namespace Application.ErrorHandlers;

public class ConflictException: Exception
{
    public ConflictException()
    {
    }

    public ConflictException(string? message) : base(message)
    {
    }
}