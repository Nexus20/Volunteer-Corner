using Volunteer_Corner.Data.Entities.Abstract;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.Data.Entities;

public class HelpSeeker : BaseEntity
{
    public string UserId { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual List<HelpRequest>? HelpRequests { get; set; }
    public bool IsApproved { get; set; }
    public virtual List<HelpProposalResponse>? HelpProposalResponses { get; set; }
}