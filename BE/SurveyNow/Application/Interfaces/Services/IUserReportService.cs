using Application.DTOs.Request.User;
using Application.DTOs.Response.User;

namespace Application.Interfaces.Services
{
    public interface IUserReportService
    {
        Task<IEnumerable<UserReportResponse>> Get(UserReportFilter? filter);
        Task<UserReportResponse> Get(long id);
        Task Create(UserReportRequest request);
        Task<UserReportResponse> ChangeReportStatus(long id, UserReportStatusRequest status);
    }
}
