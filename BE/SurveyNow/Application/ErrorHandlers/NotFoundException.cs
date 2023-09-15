namespace Application.ErrorHandlers;

public class NotFoundException: Exception
{
    public NotFoundException()
    {
    }

    public NotFoundException(string? message) : base(message)
    {
    }
}