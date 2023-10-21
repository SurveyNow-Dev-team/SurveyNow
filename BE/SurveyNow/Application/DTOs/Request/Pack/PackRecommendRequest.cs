namespace Application.DTOs.Request.Pack
{
    public class PackRecommendRequest
    {
        public required int TotalQuestions { get; set; }
        public required int TotalParticipant { get; set; }
        public required bool ExpertParticipantOption { get; set; }
    }
}
