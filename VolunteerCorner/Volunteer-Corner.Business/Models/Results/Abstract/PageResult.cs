namespace Volunteer_Corner.Business.Models.Results.Abstract;

public class PageResult<TResult>
{
    public List<TResult> Results { get; set; }
    public int CurrentPage { get; set; }
    public int TotalCount { get; set; }
    public bool HasNextPage { get; set; }
}