using Volunteer_Corner.Business.Models.Results.Abstract;
using Volunteer_Corner.Business.Models.Results.Volunteers;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Business.Models.Results.HelpProposals;

public class HelpProposalResult : BaseResult
{
    public VolunteerResult Owner { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string? Description { get; set; }
    public HelpProposalStatus Status { get; set; }
}