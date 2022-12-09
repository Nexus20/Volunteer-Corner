using Volunteer_Corner.Data.Entities.Abstract;

namespace Volunteer_Corner.Data.Entities;

public class HelpRequestResponse : BaseEntity
{
    public string HelpRequestToId { get; set; } = null!;
    public virtual HelpRequest HelpRequestTo { get; set; } = null!;
    public string VolunteerFromId { get; set; } = null!;
    public virtual Volunteer VolunteerFrom { get; set; } = null!;

    public string? IncludedHelpProposalId { get; set; }
    public virtual HelpProposal? IncludedHelpProposal { get; set; }
    
    public string Comment { get; set; } = null!;
    public bool SelectedByHelpSeeker { get; set; }
}