using Volunteer_Corner.Business.Models.Requests.Base;

namespace Volunteer_Corner.Business.Models.Requests.HelpSeekers;

public class GetHelpSeekersPageRequest : GetPageRequest, IGetHelpSeekersRequest
{
    public string? SearchString { get; set; }
    public bool? IsApproved { get; set; }
}