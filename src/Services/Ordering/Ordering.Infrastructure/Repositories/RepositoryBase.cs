using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Common;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories;

public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
{
    protected readonly OrderDbContext Context;

    protected RepositoryBase(OrderDbContext context)
    {
        Context = context;
    }
    
    public async Task<IReadOnlyList<T>> GetAllAsync() => await Context.Set<T>().ToListAsync();

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await Context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(
        Expression<Func<T, bool>>? predicate, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, 
        string? includeString,
        bool disableTracking = true)
    {
        IQueryable<T> query = Context.Set<T>();

        if (disableTracking) query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString)) query.Include(includeString);

        if (orderBy is not null)
        {
            return await orderBy(query).ToListAsync();
        }

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(
        Expression<Func<T, bool>>? predicate, 
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, 
        List<Expression<Func<T, object>>>? includes, 
        bool disableTracking = true)
    {
        IQueryable<T> query = Context.Set<T>();

        if (disableTracking) query.AsNoTracking();

        if (includes is not null)
        {
            query = includes.Aggregate(query, (current, expression) => current.Include(expression));
        }
        
        if (orderBy is not null)
        {
            return await orderBy(query).ToListAsync();
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await Context.Set<T>().FindAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        Context.Set<T>().Add(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        Context.Set<T>().Update(entity);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        Context.Set<T>().Remove(entity);
        await Context.SaveChangesAsync();
    }
}