using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Survey;

public class SectionRequest
{
    [Required(ErrorMessage = "Section order is required!")]
    [Range(1, 20, ErrorMessage = "Section order must be greater than 0 and can not exceed 20.")]
    public int Order { get; set; }

    [StringLength(300, ErrorMessage = "Title can not exceed 300 character.")]
    public string? Title { get; set; }

    [StringLength(3000, ErrorMessage = "Description can not exceed 3000 character.")]
    public string? Description { get; set; }

    public ICollection<QuestionRequest> Questions { get; set; } = new List<QuestionRequest>();
}