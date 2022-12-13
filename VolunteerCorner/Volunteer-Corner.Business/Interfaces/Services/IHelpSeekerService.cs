using Volunteer_Corner.Business.Models.Requests.HelpSeekers;
using Volunteer_Corner.Business.Models.Results.Abstract;
using Volunteer_Corner.Business.Models.Results.HelpRequests;
using Volunteer_Corner.Business.Models.Results.HelpSeekers;

namespace Volunteer_Corner.Business.Interfaces.Services;

public interface IHelpSeekerService
{
    Task<List<HelpSeekerResult>> GetAllHelpSeekers(GetAllHelpSeekersRequest request);
    Task<HelpSeekerResult> GetHelpSeekerById(string helpSeekerId);
    Task<bool> ChangeApprovalStatus(string helpSeekerId);
    Task<List<HelpRequestResult>> GetOwnHelpRequestsAsync(string helpSeekerId);
    Task<HelpRequestWithHelpResponsesResult> GetOwnHelpRequestByIdAsync(string helpSeekerId, string helpRequestId);
    Task<PageResult<HelpSeekerResult>> GetHelpSeekersPage(GetHelpSeekersPageRequest request);
}