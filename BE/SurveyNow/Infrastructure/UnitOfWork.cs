using Application;
using Application.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;
    private ILogger<UnitOfWork> _logger;

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
    public IPointPurchaseRepository PointPurchase { get; }
    public IPositionRepository PositionRepository { get; }
    public IProvinceRepository ProvinceRepository { get; }
    public IQuestionRepository QuestionRepository { get; }
    public IRowOptionRepository RowOptionRepository { get; }
    public ISectionRepository SectionRepository { get; }
    public ISurveyRepository SurveyRepository { get; }
    public IUserRepository UserRepository { get; }
    public IUserReportRepository UserReportRepository { get; }
    public IUserSurveyRepository UserSurveyRepository { get; }

    public UnitOfWork(AppDbContext context, ILogger<UnitOfWork> logger,
        IAddressRepository addressRepository, IAnswerRepository answerRepository,
        IAnswerOptionRepository answerOptionRepository, ICityRepository cityRepository,
        IColumnOptionRepository columnOptionRepository, IDistrictRepository districtRepository,
        IFieldRepository fieldRepository, IHobbyRepository hobbyRepository,
        IPackPurchaseRepository packPurchaseRepository, IPaymentRepository paymentRepository,
        IPointHistoryRepository pointHistoryRepository, IPointPurchaseRepository pointPurchase,
        IPositionRepository positionRepository, IProvinceRepository provinceRepository,
        IQuestionRepository questionRepository, IRowOptionRepository rowOptionRepository,
        ISectionRepository sectionRepository, ISurveyRepository surveyRepository, IUserRepository userRepository,
        IUserReportRepository userReportRepository, IUserSurveyRepository userSurveyRepository)
    {
        _context = context;
        _logger = logger;
        AddressRepository = addressRepository;
        AnswerRepository = answerRepository;
        AnswerOptionRepository = answerOptionRepository;
        CityRepository = cityRepository;
        ColumnOptionRepository = columnOptionRepository;
        DistrictRepository = districtRepository;
        FieldRepository = fieldRepository;
        HobbyRepository = hobbyRepository;
        PackPurchaseRepository = packPurchaseRepository;
        PaymentRepository = paymentRepository;
        PointHistoryRepository = pointHistoryRepository;
        PointPurchase = pointPurchase;
        PositionRepository = positionRepository;
        ProvinceRepository = provinceRepository;
        QuestionRepository = questionRepository;
        RowOptionRepository = rowOptionRepository;
        SectionRepository = sectionRepository;
        SurveyRepository = surveyRepository;
        UserRepository = userRepository;
        UserReportRepository = userReportRepository;
        UserSurveyRepository = userSurveyRepository;
    }

    public async Task<int> SaveChangeAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await _transaction.CommitAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error when commit transaction.\nDate:{DateTime.UtcNow}");
            throw;
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            await _transaction.RollbackAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error when roll back transaction.\nDate:{DateTime.UtcNow}");
            throw;
        }
    }

    //implement Dispose pattern
    private bool _disposed;

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }

        await _context.DisposeAsync();
    }
    //implement Dispose pattern
}