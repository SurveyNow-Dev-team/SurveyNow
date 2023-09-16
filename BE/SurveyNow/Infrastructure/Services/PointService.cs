using Application;
using Application.DTOs.Request;
using Application.DTOs.Request.Point;
using Application.DTOs.Response;
using Application.DTOs.Response.Pack;
using Application.DTOs.Response.Point.History;
using Application.DTOs.Response.Survey;
using Application.DTOs.Response.Transaction;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Services
{
    public class PointService : IPointService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PointService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagingResponse<ShortPointHistoryResponse>?> GetPaginatedPointHistoryListAsync(long userId, PointHistoryType type, PointDateFilterRequest dateFilter, PointValueFilterRequest valueFilter, PointSortOrderRequest sortOrder, PagingRequest pagingRequest)
        {
            var pageHistories = await _unitOfWork.PointHistoryRepository.GetPointHistoryPaginatedAsync(userId, type, dateFilter, valueFilter, sortOrder, pagingRequest);
            if(pageHistories == null)
            {
                return null;
            }
            PagingResponse<ShortPointHistoryResponse> result = _mapper.Map<PagingResponse<ShortPointHistoryResponse>>(pageHistories);
            return result;
        }

        public async Task<BasePointHistoryResponse?> GetPointHistoryDetailAsync(long id)
        {
            PointHistory? pointHistory = await _unitOfWork.PointHistoryRepository.GetByIdAsync(id);
            if(pointHistory == null)
            {
                return null;
            }
            switch (pointHistory.PointHistoryType)
            {
                case PointHistoryType.PurchasePoint:
                    Transaction? transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(pointHistory.PointPurchaseId);
                    TransactionResponse transactionResponse = _mapper.Map<TransactionResponse>(transaction);
                    PointPurchaseDetailResponse purchaseResult = _mapper.Map<PointPurchaseDetailResponse>(pointHistory);
                    purchaseResult.Transaction = transactionResponse;
                    return purchaseResult;
                case PointHistoryType.RedeemPoint:
                    transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(pointHistory.PointPurchaseId);
                    transactionResponse = _mapper.Map<TransactionResponse>(transaction);
                    PointPurchaseDetailResponse redeemResult = _mapper.Map<PointPurchaseDetailResponse>(pointHistory);
                    redeemResult.Transaction = transactionResponse;
                    return redeemResult;
                case PointHistoryType.DoSurvey:
                    Survey? survey = await _unitOfWork.SurveyRepository.GetByIdAsync(pointHistory.SurveyId);
                    ShortSurveyResponse surveyResponse = _mapper.Map<ShortSurveyResponse>(survey);
                    PointDoSurveyDetailResponse surveyResult = _mapper.Map<PointDoSurveyDetailResponse>(pointHistory);
                    surveyResult.Survey = surveyResponse;
                    return surveyResult;
                case PointHistoryType.PackPurchase:
                    PackPurchase? packPurchase = await _unitOfWork.PackPurchaseRepository.GetByIdAsync(pointHistory.PackPurchaseId);
                    PackPurchaseResponse packPurchaseResponse = _mapper.Map<PackPurchaseResponse>(packPurchase);
                    PointPackPurchaseDetailResponse packResult = _mapper.Map<PointPackPurchaseDetailResponse>(pointHistory);
                    packResult.PackPurchase = packPurchaseResponse;
                    return packResult;
                default:
                    // Refund point, Gift point and Receiving Point
                    // will be added later on
                    return null;
            }
        }

    }
}
