using System.Linq.Expressions;
using Application.DTOs.Response;

namespace Application.Interfaces.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<IEnumerable<T>> Get(
        Expression<Func<T, bool>>? filter,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        string includeProperties);

    Task<PagingResponse<T>> GetPaginateAsync(
        Expression<Func<T, bool>>? filter,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        string includeProperties,
        int? page,
        int? size
    );

    Task<T?> GetByIdAsync(object id);

    Task<T?> GetByIdAsync(object id, string includeProperties = "");

    Task<List<T>> GetAllAsync();

    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, T? entityFilter = null, string? includeProperties = "");

    Task AddAsync(T entity);

    void Update(T entity);

    Task DeleteByIdAsync(object id);

    Task<bool> ExistById(object id);

    Task<T> AddAsyncReturnEntity(T entity);
}