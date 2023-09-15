using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class AddressRepository: BaseRepository<Address>, IAddressRepository
{
    public AddressRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
    }
}