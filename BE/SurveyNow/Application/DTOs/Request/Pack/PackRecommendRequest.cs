namespace Application.DTOs.Request.Pack
{
    public class PackRecommendRequest
    {
        public required int TotalQuestions { get; set; }
        public required bool ExpertParticipantOption { get; set; }
    }
}
