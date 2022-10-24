namespace Volunteer_Corner.Data.Entities.Abstract;

public class BaseEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}