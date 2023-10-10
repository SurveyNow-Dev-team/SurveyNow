using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(AppDbContext context, ILogger<BaseRepository<Transaction>> logger) : base(context, logger)
    {
    }

    public async Task<bool> CheckExistPendingRedeemOrderAsync()
    {
        return await _dbSet.AnyAsync(t => t.TransactionType == Domain.Enums.TransactionType.RedeemGift && t.Status == Domain.Enums.TransactionStatus.Pending);
    }
}