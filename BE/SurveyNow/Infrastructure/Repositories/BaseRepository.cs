using System.Linq.Expressions;
using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> Get(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<T> query = _dbSet;

        try
        {
            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                         (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }
        catch (Exception e)
        {
            // _logger.LogError(e, $"Error when filter data of {typeof(T)} entity.");
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public virtual async Task<PagingResponse<T>> GetPaginate(
        Expression<Func<T, bool>>? filter,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        string includeProperties,
        int page,
        int size)
    {
        IQueryable<T> query = _dbSet;
        PagingResponse<T> result = new PagingResponse<T>();

        try
        {
            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                         (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            result.TotalRecords = await query.CountAsync();

            if (orderBy != null)
            {
                orderBy(query);
            }

            query.Skip(page * size).Take(size);
            result.CurrentPage = page;
            result.RecordsPerPage = size;
            result.TotalPages = (int)Math.Ceiling((double)result.TotalRecords / size);
            result.Results = await query.ToListAsync();
            return result;
        }
        catch (Exception e)
        {
            // _logger.LogError(e, $"Error when filter data of {typeof(T)} entity.");
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<T?> GetByIdAsync(object? id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public async Task DeleteByIdAsync(object id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    public async Task<bool> ExistById(object id)
    {
        var user = await GetByIdAsync(id);
        return user != null;
    }
}