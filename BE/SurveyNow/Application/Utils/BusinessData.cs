using Application.DTOs.Response.Pack;
using Domain.Enums;

namespace Application.Utils
{
    public static class BusinessData
    {
        #region Packs
        public static readonly PackInformation BasicPack = new PackInformation()
        {
            Name = "Basic Pack",
            PackType = PackType.Basic,
            Benefits = new List<string>()
            {
                "Up to 20 questions",
            },
        };

        public static readonly PackInformation MediumPack = new PackInformation()
        {
            Name = "Medium Pack",
            PackType = PackType.Medium,
            Benefits = new List<string>()
            {
                "Up to 40 questions",
            },
        };

        public static readonly PackInformation AdvancedPack = new PackInformation()
        {
            Name = "Advanced Pack",
            PackType = PackType.Advanced,
            Benefits = new List<string>()
            {
                "More than 40 questions",
                "More percise participants",
            },
        };

        public static readonly PackInformation ExpertPack = new PackInformation()
        {
            Name = "Expert Pack",
            PackType = PackType.Expert,
            Benefits = new List<string>()
            {
                "Unlimited questions",
                "More percise participants",
                "Option to select expert paticipants"
            },
        };

        public static readonly List<PackInformation> Packs = new List<PackInformation>() { BasicPack, MediumPack, AdvancedPack, ExpertPack };
        #endregion
    }
}
