using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class PaymentRepository: BaseRepository<Payment>, IPaymentRepository
{
    protected PaymentRepository(AppDbContext context) : base(context)
    {
    }
}