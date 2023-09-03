namespace Application.Interfaces.Repositories;

public interface IBaseRepository<T> where T : class
{
    
    Task<T?> GetByIdAsync(object? id);

    Task<List<T>> GetAllAsync();

    Task AddAsync(T entity);

    void Update (T entity);

    Task DeleteByIdAsync(object id);
    
}