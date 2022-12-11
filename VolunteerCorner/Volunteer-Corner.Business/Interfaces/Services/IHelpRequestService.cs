using Microsoft.AspNetCore.Http;
using Volunteer_Corner.Business.Models.Dtos.Files;
using Volunteer_Corner.Business.Models.Requests.HelpRequests;
using Volunteer_Corner.Business.Models.Results.HelpRequests;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Business.Interfaces.Services;

public interface IHelpRequestService
{
    Task<List<HelpRequestResult>> GetAllHelpRequests(GetAllHelpRequestsRequest request);
    Task<HelpRequestResult> GetHelpRequestById(string requestId);
    Task<HelpRequestResult> CreateAsync(CreateHelpRequestRequest request, string helpRequestOwnerId);
    Task<HelpRequestStatus> ChangeStatusAsync(string id, UpdateHelpRequestStatus request);
    Task<HelpRequestResult> UpdateAsync(string id, UpdateHelpRequestRequest request);
    Task DeleteDocumentsAsync(string id, DeleteHelpRequestDocumentsRequest request);
    Task<List<HelpRequestDocumentResult>> AddDocumentsAsync(string id, List<FileDto> filesDtos);
    Task<HelpRequestResponseResult> CreateResponseAsync(string id, string volunteerId, AddHelpRequestResponseRequest request);
}