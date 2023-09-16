using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class TransactionRepository: BaseRepository<Domain.Entities.Transaction>, ITransactionRepository
{
    public TransactionRepository(AppDbContext context, ILogger<BaseRepository<Transaction>> logger) : base(context, logger)
    {
    }
}