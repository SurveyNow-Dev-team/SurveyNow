using Application;
using Application.DTOs.Request.Pack;
using Application.DTOs.Response.Pack;
using Application.Interfaces.Services;
using Application.Utils;
using AutoMapper;
using Domain.Enums;

namespace Infrastructure.Services
{
    public class PackService : IPackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public PackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<decimal> CalculatePackPriceAsync(PackType packType, int participants)
        {
            if(participants <= 0)
            {
                throw new ArgumentOutOfRangeException($"Invalid number of participannts: {participants}");
            }

            decimal commission, userValue, userCost;
            switch (packType)
            {
                case PackType.Basic:
                    commission = 30m;
                    userValue = 0.5m;
                    userCost = userValue * participants;
                    return commission + userCost;

                case PackType.Medium:
                    commission = 50m;
                    userValue = 0.7m;
                    userCost = userValue * participants;
                    return commission + userCost;

                case PackType.Advanced:
                    commission = 80m;
                    userValue = 1m;
                    userCost = userValue * participants;
                    return commission + userCost;

                case PackType.Expert:
                    userValue = 50m;
                    userCost = userValue * participants;
                    commission = userCost / 100 * 5;
                    return commission + userCost;

                default:
                    throw new ArgumentException("Invalid pack type. Failed to calculate pack price");
            }
        }

        public async Task<List<PackInformation>> GetRecommendedPacksAsync(PackRecommendRequest request)
        {
            try
            {
                if (request.ExpertParticipantOption)
                {
                    return new List<PackInformation>() { BusinessData.ExpertPack };
                }

                if(request.TotalQuestions <= 20)
                {
                    return BusinessData.Packs;
                }
                else if(request.TotalQuestions <= 40)
                {
                    return new List<PackInformation>() { BusinessData.MediumPack, BusinessData.AdvancedPack, BusinessData.ExpertPack };
                }
                else
                {
                    return new List<PackInformation>() {BusinessData.AdvancedPack, BusinessData.ExpertPack };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
