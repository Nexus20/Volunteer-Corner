using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Business.Models.Requests.HelpRequests;

public interface IGetHelpRequestsRequest
{
    public string? OwnerId { get; set; }
    public string? SearchString { get; set; }
    public HelpRequestStatus? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}