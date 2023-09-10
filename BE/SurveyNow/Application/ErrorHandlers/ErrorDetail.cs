using System.Text.Json;

namespace Application.ErrorHandlers;

public class ErrorDetail
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string? Error { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}