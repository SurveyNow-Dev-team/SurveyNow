using Application;
using Application.DTOs.Request;
using Application.DTOs.Request.Point;
using Application.DTOs.Response;
using Application.DTOs.Response.Point;
using Application.Interfaces.Services;
using AutoMapper;

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



        #region Purchase
        public async Task<PointPurchaseDetailResponse?> GetPointPurchaseDetailAsync(long id)
        {
            var pointHistory = await _unitOfWork.PointHistoryRepository.GetPointPurchaseDetailAsync(id);
            if(pointHistory != null)
            {
                var result = _mapper.Map<PointPurchaseDetailResponse>(pointHistory);

                var pointPurchase = await _unitOfWork.TransactionRepository.GetByIdAsync(pointHistory.PointPurchaseId);
                var shortPointPurchase = _mapper.Map<ShortPointPurchaseResponse>(pointPurchase);
                result.PointPurchase = shortPointPurchase;

                return result;
            }
            return null;
        }

        public async Task<PagingResponse<PointPurchaseResponse>?> GetPointPurchasesFilteredAsync(PointDateFilterRequest dateFilter, PointValueFilterRequest valueFilter, PointSortOrderRequest sortOrder, PagingRequest pagingRequest, long userId)
        {
            var itemsList = await _unitOfWork.PointHistoryRepository.GetPointPurchasesFilteredAsync(dateFilter, valueFilter, sortOrder, pagingRequest, userId);
            if(itemsList != null)
            {
                var listResult = _mapper.Map<List<PointPurchaseResponse>>(itemsList);
                var pagingResult = new PagingResponse<PointPurchaseResponse>()
                {
                    CurrentPage = (int)pagingRequest.Page,
                    RecordsPerPage = (int)pagingRequest.RecordsPerPage,
                    Results = listResult,
                };
                return pagingResult;
            }
            else
            {
                return null;
            }
            
        }
        #endregion

        #region Redeem
        public async Task<PointRedeemDetailResponse?> GetPointRedeemDetailAsync(long id)
        {
            var pointHistory = await _unitOfWork.PointHistoryRepository.GetPointRedeemDetailAsync(id);
            if(pointHistory != null)
            {
                var result = _mapper.Map<PointRedeemDetailResponse>(pointHistory);

                var pointPurchase = await _unitOfWork.TransactionRepository.GetByIdAsync(pointHistory.PointPurchaseId);
                var shortPointPurchase = _mapper.Map<ShortPointPurchaseResponse>(pointPurchase);
                result.PointPurchase = shortPointPurchase;

                return result;
            }
            return null;
        }
        #endregion
    }
}
