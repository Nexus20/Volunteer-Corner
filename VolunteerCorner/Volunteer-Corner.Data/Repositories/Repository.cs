using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Volunteer_Corner.Data.Entities.Abstract;
using Volunteer_Corner.Data.Interfaces;

namespace Volunteer_Corner.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext Db;

    public Repository(ApplicationDbContext db)
    {
        Db = db;
    }

    public Task<List<TEntity>> GetAllAsync()
    {
        return Db.Set<TEntity>().ToListAsync();
    }

    public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return Db.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string? includeString = null,
        bool disableTracking = true)
    {
        var query = Db.Set<TEntity>().AsQueryable();

        if (predicate != null)
            query = query.Where(predicate);

        if(!string.IsNullOrEmpty(includeString))
            query = query.Include(includeString);

        if (disableTracking)
            query = query.AsNoTracking();

        if (orderBy != null)
            query = orderBy(query);

        return query.ToListAsync();
    }

    public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        List<Expression<Func<TEntity, object>>>? includes = null, bool disableTracking = true)
    {
        var query = Db.Set<TEntity>().AsQueryable();

        if (predicate != null)
            query = query.Where(predicate);

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);
        

        if (disableTracking)
            query = query.AsNoTracking();

        if (orderBy != null)
            query = orderBy(query);

        return query.ToListAsync();
    }

    public virtual Task<TEntity?> GetByIdAsync(string id)
    {
        return Db.Set<TEntity>().FirstOrDefaultAsync(i => i.Id == id);
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await Db.Set<TEntity>().AddAsync(entity);
        await Db.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        Db.Entry(entity).State = EntityState.Modified;
        await Db.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        Db.Set<TEntity>().Remove(entity);
        await Db.SaveChangesAsync();
    }
    
    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return Db.Set<TEntity>().AnyAsync(predicate);
    }
}