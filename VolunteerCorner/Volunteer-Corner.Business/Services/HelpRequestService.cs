using System.Linq.Expressions;
using System.Net.Http.Headers;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volunteer_Corner.Business.Exceptions;
using Volunteer_Corner.Business.Infrastructure.Expressions;
using Volunteer_Corner.Business.Interfaces.Infrastructure;
using Volunteer_Corner.Business.Interfaces.Services;
using Volunteer_Corner.Business.Models.Dtos.Files;
using Volunteer_Corner.Business.Models.Requests.HelpRequests;
using Volunteer_Corner.Business.Models.Results.HelpRequests;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Enums;
using Volunteer_Corner.Data.Interfaces;

namespace Volunteer_Corner.Business.Services;

public class HelpRequestService : IHelpRequestService
{
    private readonly IMapper _mapper;
    private readonly IHelpRequestRepository _helpRequestRepository;
    private readonly IRepository<HelpSeeker> _helpSeekerRepository;
    private readonly ILogger<HelpRequestService> _logger;
    private readonly IFileStorageService _fileStorageService;
    private readonly IRepository<HelpProposal> _helpProposalRepository;
    private readonly IHelpRequestResponseRepository _helpRequestResponseRepository;

    public HelpRequestService(IHelpRequestRepository helpRequestRepository, IMapper mapper,
        IRepository<HelpSeeker> helpSeekerRepository, ILogger<HelpRequestService> logger,
        IFileStorageService fileStorageService, IRepository<HelpProposal> helpProposalRepository,
        IHelpRequestResponseRepository helpRequestResponseRepository)
    {
        _helpRequestRepository = helpRequestRepository;
        _mapper = mapper;
        _helpSeekerRepository = helpSeekerRepository;
        _logger = logger;
        _fileStorageService = fileStorageService;
        _helpProposalRepository = helpProposalRepository;
        _helpRequestResponseRepository = helpRequestResponseRepository;
    }

    public async Task<List<HelpRequestResult>> GetAllHelpRequests(GetAllHelpRequestsRequest request)
    {
        var predicate = CreateFilterPredicate(request);
        var source = await _helpRequestRepository.GetAsync(predicate);

        var result = _mapper.Map<List<HelpRequest>, List<HelpRequestResult>>(source);
        return result;
    }

    public async Task<HelpRequestResult> GetHelpRequestById(string requestId)
    {
        var source = await _helpRequestRepository.GetByIdAsync(requestId);

        if (source == null)
            throw new NotFoundException(nameof(HelpRequest), requestId);
        
        var result = _mapper.Map<HelpRequest, HelpRequestResult>(source);
        return result;
    }
    
    public async Task<HelpRequestResult> UpdateAsync(string id, UpdateHelpRequestRequest request)
    {
        var helpRequestToUpdate = await _helpRequestRepository.GetByIdAsync(id);

        if (helpRequestToUpdate == null)
            throw new NotFoundException(nameof(HelpRequest), id);

        _mapper.Map<UpdateHelpRequestRequest, HelpRequest>(request, helpRequestToUpdate);

        await _helpRequestRepository.UpdateAsync(helpRequestToUpdate);
        var result = _mapper.Map<HelpRequest, HelpRequestResult>(helpRequestToUpdate);
        return result;
    }

    public async Task DeleteDocumentsAsync(string id, DeleteHelpRequestDocumentsRequest request)
    {
        var helpRequestToUpdate = await _helpRequestRepository.GetByIdAsync(id);

        if (helpRequestToUpdate == null)
            throw new NotFoundException(nameof(HelpRequest), id);

        if (helpRequestToUpdate.AdditionalDocuments?.Any() == false)
            throw new ValidationException("No documents to delete");

        var helpRequestToUpdateDocumentsIds = helpRequestToUpdate.AdditionalDocuments
            .Select(x => x.Id)
            .ToList();

        if (request.DocumentsIds.Any(x => !helpRequestToUpdateDocumentsIds.Contains(x)))
            throw new ValidationException("One of the documents ids is invalid");

        var filesToDelete = helpRequestToUpdate.AdditionalDocuments
            .Where(x => request.DocumentsIds.Contains(x.Id))
            .ToList();

        await _fileStorageService.DeleteAsync(new UrlsDto(filesToDelete.Select(x => x.FilePath).ToList()));
        await _helpRequestRepository.DeleteDocumentsAsync(filesToDelete);
    }

    public async Task<List<HelpRequestDocumentResult>> AddDocumentsAsync(string id, List<FileDto> filesDtos)
    {
        if (filesDtos.Any(x => x.Content.Length == 0))
        {
            throw new ValidationException("Files cannot be empty");
        }

        var validTypes = new[] { "application/pdf" };
        
        if (filesDtos.Any(x => !validTypes.Contains(x.ContentType)))
        {
            throw new ValidationException("Invalid file types");
        }
        
        var helpRequestToUpdate = await _helpRequestRepository.GetByIdAsync(id);

        if (helpRequestToUpdate == null)
            throw new NotFoundException(nameof(HelpRequest), id);

        if (helpRequestToUpdate.AdditionalDocuments?.Any() == false)
        {
            helpRequestToUpdate.AdditionalDocuments = new List<HelpRequestDocument>();
        }
        
        var urls = await _fileStorageService.UploadAsync(filesDtos);
        
        var helpRequestDocuments = new List<HelpRequestDocument>();

        foreach (var fileUrl in urls.Urls)
        {
            var helpRequestDocument = new HelpRequestDocument()
            {
                FilePath = fileUrl,
                HelpRequestId = helpRequestToUpdate.Id
            };
            
            helpRequestDocuments.Add(helpRequestDocument);
        }

        await _helpRequestRepository.AddDocumentsAsync(helpRequestDocuments);
        var result = _mapper.Map<List<HelpRequestDocument>, List<HelpRequestDocumentResult>>(helpRequestDocuments);
        return result;
    }

    public async Task<HelpRequestResult> CreateAsync(CreateHelpRequestRequest request, string helpRequestOwnerId,
        IFormFileCollection files, string directoryToSave)
    {
        var owner = await _helpSeekerRepository.GetByIdAsync(helpRequestOwnerId);

        if (owner == null)
        {
            throw new NotFoundException(nameof(HelpSeeker), helpRequestOwnerId);
        }

        var helpRequest = _mapper.Map<CreateHelpRequestRequest, HelpRequest>(request);
        helpRequest.Owner = owner;
        helpRequest.Status = HelpRequestStatus.Active;

        if (files?.Any() == true)
        {
            helpRequest.AdditionalDocuments = new List<HelpRequestDocument>();
            
            foreach (var file in files)
            {
                var folderName = Path.Combine("Resources", "Documents", helpRequest.Id);
                var pathToSave = Path.Combine(directoryToSave, folderName);

                if (!Directory.Exists(pathToSave))
                {
                    var dirInfo = new DirectoryInfo(pathToSave);
                    dirInfo.Create();
                }
                
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    
                    await using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    
                    helpRequest.AdditionalDocuments.Add(new HelpRequestDocument()
                    {
                        FilePath = dbPath,
                        HelpRequestId = helpRequest.Id,
                    });
                }
            }
        }

        await _helpRequestRepository.AddAsync(helpRequest);
        
        var result = _mapper.Map<HelpRequest, HelpRequestResult>(helpRequest);

        return result;
    }

    public async Task<HelpRequestStatus> ChangeStatusAsync(string id, UpdateHelpRequestStatus request)
    {
        var helpRequestToUpdate = await _helpRequestRepository.GetByIdAsync(id);

        if (helpRequestToUpdate == null)
            throw new NotFoundException(nameof(HelpRequest), id);

        if (helpRequestToUpdate.Status == HelpRequestStatus.Closed)
            throw new ValidationException("You can't change status of the closed request");

        if (helpRequestToUpdate.Status == HelpRequestStatus.Canceled && request.NewStatus == HelpRequestStatus.Closed)
            throw new ValidationException("Canceled request can't be set to closed");
        
        helpRequestToUpdate.Status = request.NewStatus;

        await _helpRequestRepository.UpdateAsync(helpRequestToUpdate);
        _logger.LogInformation("Request #{RequestId} has been updated with new status: {RequestNewStatus}", id, request.NewStatus);
        return request.NewStatus;
    }
    
    public async Task<HelpRequestResponseResult> CreateResponseAsync(string id, string volunteerId, AddHelpRequestResponseRequest request)
    {
        var helpRequest = await _helpRequestRepository.GetByIdAsync(id);
        
        if (helpRequest == null)
            throw new NotFoundException(nameof(HelpRequest), id);
        
        if (helpRequest.Status is HelpRequestStatus.Closed or HelpRequestStatus.Canceled)
            throw new ValidationException("You can't add response to canceled or closed request");

        var responseEntity = new HelpRequestResponse()
        {
            Comment = request.Comment,
            VolunteerFromId = volunteerId,
            HelpRequestToId = id
        };
        
        if (!string.IsNullOrWhiteSpace(request.IncludedHelpProposalId))
        {
            var helpProposalToInclude = await _helpProposalRepository.GetByIdAsync(request.IncludedHelpProposalId);
            
            if(helpProposalToInclude == null)
                throw new ValidationException("Help proposal you want to add to your response doesn't exist");

            if (helpProposalToInclude.OwnerId != volunteerId)
                throw new ValidationException("You can't add not your own help proposals");

            var helpProposalAlreadyIncluded = await _helpRequestResponseRepository.ExistsAsync(x =>
                x.HelpRequestToId == id && x.IncludedHelpProposalId == request.IncludedHelpProposalId);

            if (helpProposalAlreadyIncluded)
                throw new ValidationException("Help proposal you want to include is already included");

            responseEntity.IncludedHelpProposalId = request.IncludedHelpProposalId;
        }

        await _helpRequestResponseRepository.AddAsync(responseEntity);
        _logger.LogInformation("New response to help request {HelpRequestId} has been successfully added. Response id: {HelpRequestResponseId}", id, responseEntity.Id);
        var source = await _helpRequestResponseRepository.GetByIdWithDetailsAsync(responseEntity.Id);
        var result = _mapper.Map<HelpRequestResponse, HelpRequestResponseResult>(source!);
        return result;
    }

    private static Expression<Func<HelpRequest, bool>>? CreateFilterPredicate(GetAllHelpRequestsRequest request)
    {
        Expression<Func<HelpRequest, bool>>? predicate = null;

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            Expression<Func<HelpRequest, bool>> searchStringExpression = x =>
                x.Description != null && x.Description.Contains(request.SearchString) ||
                x.Name.Contains(request.SearchString);

            predicate = ExpressionsHelper.And(predicate, searchStringExpression);
        }

        if (request.Status.HasValue && Enum.IsDefined(request.Status.Value))
        {
            Expression<Func<HelpRequest, bool>> statusPredicate = x => x.Status == request.Status.Value;
            predicate = ExpressionsHelper.And(predicate, statusPredicate);
        }

        if (request.StartDate.HasValue && request.EndDate.HasValue && request.StartDate < request.EndDate)
        {
            Expression<Func<HelpRequest, bool>> dateExpression = x => x.CreatedDate > request.StartDate.Value
                                                                      && x.CreatedDate < request.EndDate.Value;
            predicate = ExpressionsHelper.And(predicate, dateExpression);
        }

        if (!string.IsNullOrWhiteSpace(request.OwnerId))
        {
            Expression<Func<HelpRequest, bool>> ownerPredicate = x => x.OwnerId == request.OwnerId;
            predicate = ExpressionsHelper.And(predicate, ownerPredicate);
        }

        return predicate;
    }
}