using Application;
using Application.DTOs.Request.Pack;
using Application.DTOs.Response.Pack;
using Application.Interfaces.Services;
using Application.Utils;
using AutoMapper;
using Domain.Entities;
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

        public async Task PurchasePackAsync(User user, PackPurchaseRequest purchaseRequest)
        {
            // Calculate pack price
            decimal cost = await CalculatePackPriceAsync(purchaseRequest.PackType, purchaseRequest.TotalParticipants);

            // Check for survey conditions
            Survey survey = await _unitOfWork.SurveyRepository.GetByIdAsync(purchaseRequest.SurveyId);
            if (survey == null)
            {
                throw new ArgumentNullException("Failed to located the associated survey.");
            }
            else if (survey.Status == SurveyStatus.PackPurchased)
            {
                throw new OperationCanceledException($"A pack has been purchase for this survey. Survey's ID: {survey.Id}");
            }
            // Check user point balance
            if (user.Point < cost)
            {
                throw new OperationCanceledException($"Insufficient user point. Required: {cost}; Balance: {user.Point}");
            }
            try
            {
                PointHistory pointHistory = new PointHistory() 
                {
                    UserId = user.Id,
                    SurveyId = purchaseRequest.SurveyId,
                    Point = cost,
                    PointHistoryType = PointHistoryType.PackPurchase,
                    Date = DateTime.UtcNow,
                    Description = EnumUtil.GeneratePointHistoryDescription(PointHistoryType.PackPurchase, user.Id, cost, purchaseRequest.SurveyId, purchaseRequest.PackType),
                    Status = TransactionStatus.Success,
                };
                PackPurchase packPurchase = new PackPurchase()
                {
                    UserId = user.Id,
                    Date = DateTime.UtcNow,
                    PackType = purchaseRequest.PackType,
                    Point = cost,
                    Status = TransactionStatus.Success,
                    SurveyId = survey.Id,
                };
                // Begin transaction
                await _unitOfWork.BeginTransactionAsync();

                // Update user point
                await _unitOfWork.UserRepository.UpdateUserPoint(user.Id, UserPointAction.DecreasePoint, cost);
                // Adding PackPurchase
                var packPurchaseEntity = await _unitOfWork.PackPurchaseRepository.AddPackPurchaseAsync(packPurchase);
                await _unitOfWork.SaveChangeAsync();
                // Adding PointHistory
                pointHistory.PackPurchaseId = packPurchaseEntity.Id;
                await _unitOfWork.PointHistoryRepository.AddPointHistoryAsync(pointHistory);
                // Change survey's status
                survey.Status = SurveyStatus.PackPurchased;
                survey.PackType = purchaseRequest.PackType;
                survey.Point = cost;
                _unitOfWork.SurveyRepository.Update(survey);
                await _unitOfWork.SurveyRepository.UpdateTotalParticipant((int)survey.Id, purchaseRequest.TotalParticipants);

                // Commit transaction
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new InvalidOperationException($"Failed to process user's pack purchase request\n{ex.Message}");
            }
        }
    }
}
