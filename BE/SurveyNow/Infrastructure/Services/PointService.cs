using Application;
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
        public async Task<PointPurchaseDetailResponse> GetPointPurchaseDetail(long id)
        {
            var pointHistory = await _unitOfWork.PointHistoryRepository.GetPointPurchaseDetailAsync(id);
            var result = _mapper.Map<PointPurchaseDetailResponse>(pointHistory);

            var pointPurchase = await _unitOfWork.PointPurchase.GetByIdAsync(pointHistory.PointPurchaseId);
            var shortPointPurchase = _mapper.Map<ShortPointPurchaseResponse>(pointPurchase);
            
            result.PointPurchase = shortPointPurchase;
            return result;
        }
        #endregion
        #region Redeem
        #endregion
    }
}
