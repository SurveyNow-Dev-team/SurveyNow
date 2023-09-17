using Application.DTOs.Request.Pack;
using Application.DTOs.Response.Pack;
using Domain.Enums;

namespace Application.Interfaces.Services
{
    public interface IPackService
    {
        Task<decimal> CalculatePackPriceAsync(PackType packType, int participants);
        Task<List<PackInformation>> GetRecommendedPacksAsync(PackRecommendRequest request);
    }
}
