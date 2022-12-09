using Volunteer_Corner.Business.Models.Results.Abstract;
using Volunteer_Corner.Business.Models.Results.HelpProposals;
using Volunteer_Corner.Business.Models.Results.Volunteers;

namespace Volunteer_Corner.Business.Models.Results.HelpRequests;

public class HelpRequestResponseResult : BaseResult
{
    public virtual HelpRequestResult HelpRequestTo { get; set; } = null!;
    public virtual VolunteerResult VolunteerFrom { get; set; } = null!;
    public virtual HelpProposalResult? IncludedHelpProposal { get; set; }
    public bool SelectedByHelpSeeker { get; set; }
}