using Application.DTOs.Response.Pack;
using Domain.Enums;

namespace Application.Utils
{
    public static class BusinessData
    {
        #region Packs
        public static readonly PackInformation BasicPack = new PackInformation()
        {
            Name = "Gói Tiết Kiệm",
            PackType = PackType.Basic,
            Benefits = new List<string>()
            {
                "Khảo sát lên tới 20 câu hỏi",
                "Tối đa 100 người khảo sát",
            },
        };

        public static readonly PackInformation MediumPack = new PackInformation()
        {
            Name = "Gói Cơ Bản",
            PackType = PackType.Medium,
            Benefits = new List<string>()
            {
                "Khảo sát lên tới 40 câu hỏi",
                "Tối đa 200 người khảo sát",
            },
        };

        public static readonly PackInformation AdvancedPack = new PackInformation()
        {
            Name = "Gói Nâng Cao",
            PackType = PackType.Advanced,
            Benefits = new List<string>()
            {
                "Khảo sát hơn 40 câu hỏi",
                "Đối tượng khảo sát chính xác hơn",
                "Tối đa 300 người khảo sát",
            },
        };

        public static readonly PackInformation ExpertPack = new PackInformation()
        {
            Name = "Gói Chuyên Gia",
            PackType = PackType.Expert,
            Benefits = new List<string>()
            {
                "Không giới hạn số lượng câu hỏi",
                "Đối tượng khảo sát chính xác hơn",
                "Lựa chọn người khảo sát là các chuyên gia",
                "Tối đa 300 người khảo sát",
            },
        };

        public static readonly List<PackInformation> Packs = new List<PackInformation>() { BasicPack, MediumPack, AdvancedPack, ExpertPack };
        #endregion
        #region Point
        public static readonly decimal BasePointVNDPrice = 1000m;
        #endregion
        #region Transaction
        public static readonly string SurveyNowMomoAccount = "0907817524";
        public static readonly string SurveyNowVnPayAccount = "0346476019";
        #endregion
    }
}
