using System.Linq.Expressions;
using System.Reflection;
using Application.DTOs.Response;
using Application.ErrorHandlers;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

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
        string includeProperties = "",
        bool disableTracking = true
    )
    {
        IQueryable<T> query = _dbSet;

        try
        {
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

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
        int? size,
        bool disableTracking = true)
    {
        IQueryable<T> query = _dbSet;
        var result = new PagingResponse<T>();

        try
        {
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query,
                (current, includeProperty) => IncludeNested(current, includeProperty));

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

    public async Task<T?> GetByIdAsync(object? id, string includeProperties = "")
    {
        IQueryable<T> query = _dbSet;
        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        var result = query.FirstOrDefault(delegate(T t)
        {
            var currentId = typeof(T).GetProperty("Id").GetValue(t);
            return currentId.Equals(id);
        });
        return result;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, T? entityFilter = null,
        string? includeProperties = "")
    {
        IQueryable<T> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (entityFilter != null)
        {
            var properties = entityFilter.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var data = entityFilter.GetType().GetProperty(property.Name)?.GetValue(entityFilter);
                if (data != null)
                {
                    Type type = data.GetType();
                    if (type == typeof(string))
                    {
                        query = query.Where(property.Name + ".ToLower().Contains(@0)", (data as string).ToLower());
                    }
                    else if (type == typeof(int) || type == typeof(long) || type == typeof(float) ||
                             type == typeof(double))
                    {
                        query = query.Where(property.Name + " == @0", data);
                    }
                    else if (type == typeof(bool))
                    {
                        query = query.Where(property.Name + " == @0", data);
                    }
                    else if (type == typeof(DateTime))
                    {
                        DateTime date = (DateTime)data;
                        query = query.Where(property.Name + "== @0", date);
                    }
                }
            }
        }

        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync();
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
        var result = query;

        try
        {
            result = includeProperties.Select(prop => typeof(T).GetProperty(prop)).Aggregate(result,
                (current, navigationProp) => current.Include(navigationProp?.Name ?? ""));
        }
        catch (Exception e)
        {
            throw new BadRequestException("Some include properties does not exist.");
        }

        return result;
    }

    public async Task<T> AddAsyncReturnEntity(T entity)
    {
        EntityEntry<T> resultEntity = await _dbSet.AddAsync(entity);
        return resultEntity.Entity;
    }
}