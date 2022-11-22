using Volunteer_Corner.Data.Entities.Abstract;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Data.Entities;

public class HelpRequest : BaseEntity
{
    public string OwnerId { get; set; } = null!;
    public virtual HelpSeeker Owner { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string? Description { get; set; }
    public HelpRequestStatus Status { get; set; }
    public virtual List<HelpRequestDocument>? AdditionalDocuments { get; set; }
}