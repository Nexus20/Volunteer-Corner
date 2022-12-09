using Volunteer_Corner.Business.Models.Requests.HelpSeekers;
using Volunteer_Corner.Business.Models.Results.HelpRequests;

namespace Volunteer_Corner.Business.Interfaces.Services;

public interface IHelpSeekerService
{
    Task<List<HelpSeekerResult>> GetAllHelpSeekers(GetAllHelpSeekersRequest request);
    Task<HelpSeekerResult> GetHelpSeekerById(string helpSeekerId);
    Task<bool> ChangeApprovalStatus(string helpSeekerId);
}