using Volunteer_Corner.Data.Entities.Abstract;

namespace Volunteer_Corner.Data.Entities;

public class HelpProposalResponse : BaseEntity
{
    public string HelpProposalToId { get; set; } = null!;
    public virtual HelpProposal HelpProposalTo { get; set; } = null!;
    public string HelpSeekerFromId { get; set; } = null!;
    public virtual HelpSeeker HelpSeekerFrom { get; set; } = null!;
    
    public string? IncludedHelpRequestId { get; set; }
    public virtual HelpRequest? IncludedHelpRequest { get; set; }
    
    public string Comment { get; set; } = null!;
    public bool SelectedByVolunteer { get; set; }
}