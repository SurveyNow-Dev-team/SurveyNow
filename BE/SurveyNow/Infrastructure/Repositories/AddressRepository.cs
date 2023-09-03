using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class AddressRepository: BaseRepository<Address>, IAddressRepository
{
    protected AddressRepository(AppDbContext context) : base(context)
    {
    }
}