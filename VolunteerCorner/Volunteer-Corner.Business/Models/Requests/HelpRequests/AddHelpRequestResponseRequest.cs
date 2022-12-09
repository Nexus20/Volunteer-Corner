using System.ComponentModel.DataAnnotations;

namespace Volunteer_Corner.Business.Models.Requests.HelpRequests;

public class AddHelpRequestResponseRequest
{
    [Required] public string Comment { get; set; } = null!;
    public string? IncludedHelpProposalId { get; set; }
}