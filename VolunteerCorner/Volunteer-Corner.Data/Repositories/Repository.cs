using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Volunteer_Corner.Data.Entities.Abstract;
using Volunteer_Corner.Data.Interfaces;

namespace Volunteer_Corner.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext _db;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<List<TEntity>> GetAllAsync()
    {
        return _db.Set<TEntity>().ToListAsync();
    }

    public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return _db.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeString = null,
        bool disableTracking = true)
    {
        var query = _db.Set<TEntity>().AsQueryable();

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

    public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        List<Expression<Func<TEntity, object>>> includes = null, bool disableTracking = true)
    {
        var query = _db.Set<TEntity>().AsQueryable();

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

    public Task<TEntity?> GetByIdAsync(string id)
    {
        return _db.Set<TEntity>().FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _db.Set<TEntity>().AddAsync(entity);
        await _db.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _db.Set<TEntity>().Remove(entity);
        await _db.SaveChangesAsync();
    }
}