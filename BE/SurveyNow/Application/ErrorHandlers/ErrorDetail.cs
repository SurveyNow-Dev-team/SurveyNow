using System.Text.Json;

namespace Application.ErrorHandlers;

public class ErrorDetail
{
    public int StatusCode { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;

    public override string ToString()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(this, options);
    }
}