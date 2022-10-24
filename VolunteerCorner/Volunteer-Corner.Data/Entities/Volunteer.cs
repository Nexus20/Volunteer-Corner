using Volunteer_Corner.Data.Entities.Abstract;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.Data.Entities;

public class Volunteer : BaseEntity
{
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
    public List<HelpProposal>? HelpProposals { get; set; }
}