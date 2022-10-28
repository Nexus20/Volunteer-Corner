using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Business.Models.Requests;

public class GetAllHelpRequestsRequest
{
    public string? SearchString { get; set; }
    public HelpRequestStatus? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}