using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class PaymentRepository: BaseRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
    }
}