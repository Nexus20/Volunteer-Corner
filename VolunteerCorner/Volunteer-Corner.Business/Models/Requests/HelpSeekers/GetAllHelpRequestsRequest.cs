namespace Volunteer_Corner.Business.Models.Requests.HelpSeekers;

public class GetAllHelpSeekersRequest : IGetHelpSeekersRequest
{
    public string? SearchString { get; set; }
    public bool? IsApproved { get; set; }
}