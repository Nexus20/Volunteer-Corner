using Volunteer_Corner.Data.Entities.Abstract;

namespace Volunteer_Corner.Data.Entities;

public class HelpProposalPhoto : BaseEntity
{
    public string HelpProposalId { get; set; } = null!;
    public virtual HelpProposal HelpProposal { get; set; } = null!;
    public string FilePath { get; set; } = null!;
}