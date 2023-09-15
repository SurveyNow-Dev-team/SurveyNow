using Application.Interfaces.Repositories;

namespace Application;

public interface IUnitOfWork: IDisposable
{
    public IAddressRepository AddressRepository { get; }
    public IAnswerRepository AnswerRepository { get; }
    public ICityRepository CityRepository { get; }
    public IDistrictRepository DistrictRepository { get; }
    public IFieldRepository FieldRepository { get; }
    public IHobbyRepository HobbyRepository { get; }
    public IPackPurchaseRepository PackPurchaseRepository { get; }
    public IPaymentRepository PaymentRepository { get; }
    public IPointHistoryRepository PointHistoryRepository { get; }
    public IPointPurchaseRepository PointPurchase { get; }
    public IPositionRepository PositionRepository { get; }
    public IProvinceRepository ProvinceRepository { get; }
    public IQuestionRepository QuestionRepository { get; }
    public IQuestionDetailRepository QuestionDetailRepository { get; }
    public ISurveyRepository SurveyRepository { get; }
    public IUserRepository UserRepository { get; }
    public IUserReportRepository UserReportRepository { get; }
    public IUserSurveyRepository UserSurveyRepository { get; }

    public Task<int> SaveChangeAsync();

    public Task BeginTransactionAsync();
    public Task CommitAsync();
    public Task RollbackAsync();
}