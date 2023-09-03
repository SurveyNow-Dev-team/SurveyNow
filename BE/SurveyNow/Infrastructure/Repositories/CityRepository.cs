using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

 public class CityRepository: BaseRepository<City>, ICityRepository
{
 protected CityRepository(AppDbContext context) : base(context)
 {
 }
}