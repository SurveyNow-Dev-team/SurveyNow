using Application.Interfaces.Repositories;

namespace Application;

public interface IUnitOfWork : IDisposable
{
    public IAddressRepository AddressRepository { get; }
    public IAnswerRepository AnswerRepository { get; }
    public IAnswerOptionRepository AnswerOptionRepository { get; }
    public ICityRepository CityRepository { get; }
    public IColumnOptionRepository ColumnOptionRepository { get; }
    public IDistrictRepository DistrictRepository { get; }
    public IFieldRepository FieldRepository { get; }
    public IHobbyRepository HobbyRepository { get; }
    public IPackPurchaseRepository PackPurchaseRepository { get; }
    public IPaymentRepository PaymentRepository { get; }
    public IPointHistoryRepository PointHistoryRepository { get; }
    public ITransactionRepository TransactionRepository { get; }
    public IPositionRepository PositionRepository { get; }
    public IProvinceRepository ProvinceRepository { get; }
    public IQuestionRepository QuestionRepository { get; }
    public IRowOptionRepository RowOptionRepository { get; }
    public ISectionRepository SectionRepository { get; }
    public ISurveyRepository SurveyRepository { get; }
    public IUserRepository UserRepository { get; }
    public IUserReportRepository UserReportRepository { get; }
    public IUserSurveyRepository UserSurveyRepository { get; }
    public IAreaCriterionRepository AreaCriterionRepository { get; }
    public ICriterionRepository CriterionRepository { get; }
    public IFieldCriterionRepository FieldCriterionRepository { get; }
    public IGenderCriterionRepository GenderCriterionRepository { get; }
    public IRelationshipCriterionRepository RelationshipCriterionRepository { get; }

    public Task<int> SaveChangeAsync();

    public Task BeginTransactionAsync();
    public Task CommitAsync();
    public Task RollbackAsync();

    public ValueTask DisposeAsync();
}