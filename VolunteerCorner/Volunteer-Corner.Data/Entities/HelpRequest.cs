using Volunteer_Corner.Data.Entities.Abstract;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Data.Entities;

public class HelpRequest : BaseEntity
{
    public string OwnerId { get; set; } = null!;
    public HelpSeeker Owner { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    public HelpRequestStatus Status { get; set; }
    public List<HelpRequestDocument>? AdditionalDocuments { get; set; }
}