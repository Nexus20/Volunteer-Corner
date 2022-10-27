using System.Linq.Expressions;
using Volunteer_Corner.Data.Entities.Abstract;
using Volunteer_Corner.Data.Interfaces;

namespace Volunteer_Corner.Data.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    public Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeString = null,
        bool disableTracking = true)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        List<Expression<Func<TEntity, object>>> includes = null, bool disableTracking = true)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity> AddAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }
}