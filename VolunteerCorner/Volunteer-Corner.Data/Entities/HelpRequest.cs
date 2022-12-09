using Volunteer_Corner.Data.Entities.Abstract;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Data.Entities;

public class HelpRequest : BaseEntity
{
    public string OwnerId { get; set; } = null!;
    public virtual HelpSeeker Owner { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string? Description { get; set; }
    public HelpRequestStatus Status { get; set; }
    public virtual List<HelpRequestDocument>? AdditionalDocuments { get; set; }
}

// public class HelpRequestResponse : BaseEntity
// {
//     public string HelpSeekerId { get; set; } = null!;
//     public virtual HelpSeeker HelpSeeker { get; set; } = null!;
//     public string VolunteerId { get; set; } = null!;
//     public virtual Volunteer Volunteer { get; set; } = null!;
//     public string Comment { get; set; } = null!;
//
//     public string? HelpProposalId { get; set; }
//     public virtual HelpProposal? HelpProposal { get; set; }
// }