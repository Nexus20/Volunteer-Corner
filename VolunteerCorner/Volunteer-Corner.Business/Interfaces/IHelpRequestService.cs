using Microsoft.AspNetCore.Http;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results.HelpRequests;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Business.Interfaces;

public interface IHelpRequestService
{
    Task<List<HelpRequestResult>> GetAllHelpRequests(GetAllHelpRequestsRequest request);
    Task<HelpRequestResult> GetHelpRequestById(string requestId);
    Task<HelpRequestResult> CreateAsync(CreateHelpRequestRequest request,
        IFormFileCollection files, string directoryToSave);
    Task<HelpRequestStatus> ChangeStatusAsync(string id, UpdateHelpRequestStatus request);
    Task<HelpRequestResult> UpdateAsync(string id, UpdateHelpRequestRequest request, IFormFileCollection files,
        string directoryToSave);
}