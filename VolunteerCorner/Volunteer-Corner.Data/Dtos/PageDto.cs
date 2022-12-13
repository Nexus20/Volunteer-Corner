using Volunteer_Corner.Data.Entities.Abstract;

namespace Volunteer_Corner.Data.Dtos;

public class PageDto<TEntity> where TEntity : BaseEntity
{
    public List<TEntity> Results { get; set; }
    public int CurrentPage { get; set; }
    public int TotalCount { get; set; }
    public bool HasNextPage { get; set; }
}