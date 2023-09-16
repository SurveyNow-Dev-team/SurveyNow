namespace Application.DTOs.Response.Survey
{
    public class ShortSurveyResponse
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public required string Status { get; set; }
    }
}
