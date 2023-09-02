using Infrastructure.Data;
using Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddDependency(this IServiceCollection services, string databaseConnection)
    {
        //Add db context
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(databaseConnection));

        //Add Mapper
        services.AddAutoMapper(typeof(UserMappingProfile));
        
        return services;
    }
}