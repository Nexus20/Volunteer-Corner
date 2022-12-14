using Volunteer_Corner.Data.Entities.Abstract;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Data.Entities;

public class HelpProposal : BaseEntity
{
    public string OwnerId { get; set; } = null!;
    public virtual Volunteer Owner { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string? Description { get; set; }
    
    public HelpProposalStatus Status { get; set; }
    public virtual List<HelpProposalPhoto>? AdditionalPhotos { get; set; }
    public virtual List<HelpProposalResponse>? Responses { get; set; }
    public virtual List<HelpRequestResponse>? IncludedToHelpRequestResponses { get; set; }
}