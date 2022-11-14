using Volunteer_Corner.Data.Entities.Abstract;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Data.Entities;

public class HelpProposal : BaseEntity
{
    public string OwnerId { get; set; } = null!;
    public Volunteer Owner { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string? Description { get; set; }
    
    public HelpProposalStatus Status { get; set; }
    public List<HelpProposalPhoto>? AdditionalPhotos { get; set; }
}