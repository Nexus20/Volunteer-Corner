using Microsoft.AspNetCore.Http;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results.HelpRequests;

namespace Volunteer_Corner.Business.Interfaces;

public interface IHelpRequestService
{
    Task<List<HelpRequestResult>> GetAllHelpRequests(GetAllHelpRequestsRequest request);
    Task<HelpRequestResult> GetHelpRequestById(string requestId);
    Task<HelpRequestResult> CreateAsync(CreateHelpRequestRequest request, IFormFileCollection files, string directoryToSave);
    Task<HelpRequestResult> Put(int id, string value);
    Task<HelpRequestResult> Put(CreateHelpRequestRequest request, IFormFileCollection files, string directoryToSave);
}