using System.Linq.Expressions;
using Application.DTOs.Response;
using Application.ErrorHandlers;
using Application.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;
    protected readonly ILogger<BaseRepository<T>> _logger;

    public BaseRepository(AppDbContext context, ILogger<BaseRepository<T>> logger)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _logger = logger;
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
            _logger.LogError(e, $"Error when filter data of {typeof(T)} entity.");
            throw;
        }
    }

    public virtual async Task<PagingResponse<T>> GetPaginateAsync(
        Expression<Func<T, bool>>? filter,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
        string includeProperties,
        int? page,
        int? size)
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
                // query = query.Include(includeProperty);
                query = IncludeNested(query, includeProperty);
            }

            result.TotalRecords = await query.CountAsync();

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (page.HasValue && size.HasValue)
            {
                if (page.Value <= 0)
                {
                    throw new BadRequestException("Page must be greater than 0.");
                }

                if (size.Value <= 0)
                {
                    throw new BadRequestException("Size must be greater than 0.");
                }

                query = query.Skip((page.Value - 1) * size.Value).Take(size.Value);
                result.CurrentPage = page.Value;
                result.RecordsPerPage = size.Value;
                result.Results = await query.ToListAsync();
                result.TotalPages = (int)Math.Ceiling((double)result.TotalRecords / size.Value);
            }
            else
            {
                result.Results = await query.ToListAsync();
            }

            
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error when filter data of {typeof(T)} entity.");
            throw new Exception(e.Message);
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

    public virtual async Task<IEnumerable<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            // query = query.Include(includeProperty);
            query = IncludeNested(query, includeProperty);
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

    private static IQueryable<T> IncludeNested<T>(IQueryable<T> query, string includeProperty) where T : class
    {
        var includeProperties = includeProperty.Split('.');
        IQueryable<T> result = query;

        try
        {
            foreach (var prop in includeProperties)
            {
                var navigationProp = typeof(T).GetProperty(prop);
                result = result.Include(navigationProp.Name);
            }
        }
        catch (Exception e)
        {
            throw new BadRequestException("Some include properties does not exist.");
        }

        return result;
    }
}