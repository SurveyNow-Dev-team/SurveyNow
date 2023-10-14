using Application;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Mappers;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDependency(this IServiceCollection services, string? databaseConnection)
    {
        //Add db context
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(databaseConnection));

        //Add repository
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserSurveyRepository, UserSurveyRepository>();
        services.AddScoped<IUserReportRepository, UserReportRepository>();
        services.AddScoped<ISurveyRepository, SurveyRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IProvinceRepository, ProvinceRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IPointHistoryRepository, PointHistoryRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPackPurchaseRepository, PackPurchaseRepository>();
        services.AddScoped<IOccupationRepository, OccupationRepository>();
        services.AddScoped<IHobbyRepository, HobbyRepository>();
        services.AddScoped<IFieldRepository, FieldRepository>();
        services.AddScoped<IDistrictRepository, DistrictRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IAnswerRepository, AnswerRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IAnswerOptionRepository, AnswerOptionRepository>();
        services.AddScoped<IColumnOptionRepository, ColumnOptionRepository>();
        services.AddScoped<IRowOptionRepository, RowOptionRepository>();
        services.AddScoped<ISectionRepository, SectionRepository>();
        services.AddScoped<IAreaCriterionRepository, AreaCriterionRepository>();
        services.AddScoped<ICriterionRepository, CriterionRepository>();
        services.AddScoped<IFieldCriterionRepository, FieldCriterionRepository>();
        services.AddScoped<IGenderCriterionRepository, GenderCriterionRepository>();
        services.AddScoped<IRelationshipCriterionRepository, RelationshipCriterionRepository>();

        //Add services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IHobbyService, HobbyService>();
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<IOccupationService, OccupationService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPhoneNumberService, PhoneNumberService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IPointService, PointService>();
        services.AddScoped<IPackService, PackService>();
        services.AddScoped<ISurveyService, SurveyService>();
        services.AddScoped<IMomoService, MomoService>();
        services.AddScoped<ITransactionService, TransactionService>();

        //Unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //Add Mapper
        services.AddAutoMapper(typeof(UserMappingProfile));
        services.AddAutoMapper(typeof(SurveyMappingProfile));
        services.AddAutoMapper(typeof(PointMappingProfile));

        return services;
    }
}