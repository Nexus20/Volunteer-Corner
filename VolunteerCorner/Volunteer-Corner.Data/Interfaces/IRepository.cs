using System.Linq.Expressions;
using Volunteer_Corner.Data.Dtos;
using Volunteer_Corner.Data.Entities.Abstract;
using Volunteer_Corner.Data.Repositories;

namespace Volunteer_Corner.Data.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

    Task<PageDto<TEntity>> GetPageAsync(int pageNumber, int takeCount,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string? includeString = null,
        bool disableTracking = true);

    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        List<Expression<Func<TEntity, object>>>? includes = null,
        bool disableTracking = true);

    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity?> GetByIdAsync(string id);
    Task<TEntity> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}