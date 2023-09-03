using Application;
using Application.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure;

public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly AppDbContext _context;

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

    public UnitOfWork(AppDbContext context, IAddressRepository addressRepository, IAnswerRepository answerRepository,
        ICityRepository cityRepository, IDistrictRepository districtRepository, IFieldRepository fieldRepository,
        IHobbyRepository hobbyRepository, IPackPurchaseRepository packPurchaseRepository,
        IPaymentRepository paymentRepository, IPointHistoryRepository pointHistoryRepository,
        IPointPurchaseRepository pointPurchase, IPositionRepository positionRepository,
        IProvinceRepository provinceRepository, IQuestionRepository questionRepository,
        IQuestionDetailRepository questionDetailRepository, ISurveyRepository surveyRepository,
        IUserRepository userRepository, IUserReportRepository userReportRepository,
        IUserSurveyRepository userSurveyRepository)
    {
        _context = context;
        AddressRepository = addressRepository;
        AnswerRepository = answerRepository;
        CityRepository = cityRepository;
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
        QuestionDetailRepository = questionDetailRepository;
        SurveyRepository = surveyRepository;
        UserRepository = userRepository;
        UserReportRepository = userReportRepository;
        UserSurveyRepository = userSurveyRepository;
    }


    public async Task<int> SaveChangeAsync()
    {
        return await _context.SaveChangesAsync();
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
        await _context.DisposeAsync();
    }
    //implement Dispose pattern
}