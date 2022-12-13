namespace Volunteer_Corner.Business.Models.Requests.HelpSeekers;

public interface IGetHelpSeekersRequest
{
    public string? SearchString { get; set; }
    public bool? IsApproved { get; set; }
}