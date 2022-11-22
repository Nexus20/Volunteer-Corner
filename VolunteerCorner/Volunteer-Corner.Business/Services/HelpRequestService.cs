﻿using System.Linq.Expressions;
using System.Net.Http.Headers;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Volunteer_Corner.Business.Exceptions;
using Volunteer_Corner.Business.Infrastructure.Expressions;
using Volunteer_Corner.Business.Interfaces;
using Volunteer_Corner.Business.Models.Requests;
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

    public HelpRequestService(IHelpRequestRepository helpRequestRepository, IMapper mapper, IRepository<HelpSeeker> helpSeekerRepository, ILogger<HelpRequestService> logger)
    {
        _helpRequestRepository = helpRequestRepository;
        _mapper = mapper;
        _helpSeekerRepository = helpSeekerRepository;
        _logger = logger;
    }

    public async Task<List<HelpRequestResult>> GetAllHelpRequests(GetAllHelpRequestsRequest request)
    {
        var predicate = CreateFilterPredicate(request);
        var source = await _helpRequestRepository.GetAsync(predicate, includes: new List<Expression<Func<HelpRequest, object>>>()
        {
            x => x.Owner,
            x => x.AdditionalDocuments
        });

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
    
    public async Task<HelpRequestResult> UpdateAsync(string id, UpdateHelpRequestRequest request, IFormFileCollection files,
        string directoryToSave)
    {
        var helpRequestToUpdate = await _helpRequestRepository.GetByIdAsync(id);

        if (helpRequestToUpdate == null)
            throw new NotFoundException(nameof(HelpRequest), id);

        _mapper.Map<UpdateHelpRequestRequest, HelpRequest>(request, helpRequestToUpdate);

        if (files?.Any() == true)
        {
            if (helpRequestToUpdate.AdditionalDocuments?.Any() == false)
            {
                helpRequestToUpdate.AdditionalDocuments = new List<HelpRequestDocument>();
            }
            
            foreach (var file in files)
            {
                var folderName = Path.Combine("Resources", "Documents", helpRequestToUpdate.Id);
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
                    
                    helpRequestToUpdate.AdditionalDocuments.Add(new HelpRequestDocument()
                    {
                        FilePath = dbPath,
                        HelpRequestId = helpRequestToUpdate.Id,
                    });
                }
            }
        }
        
        await _helpRequestRepository.UpdateAsync(helpRequestToUpdate);
        var result = _mapper.Map<HelpRequest, HelpRequestResult>(helpRequestToUpdate);
        return result;
    }
    
    public async Task<HelpRequestResult> CreateAsync(CreateHelpRequestRequest request,
        IFormFileCollection files, string directoryToSave)
    {
        var owner = await _helpSeekerRepository.GetByIdAsync(request.OwnerId);

        if (owner == null)
        {
            throw new NotFoundException(nameof(HelpSeeker), request.OwnerId);
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
        helpRequest.Owner = owner;
        var result = _mapper.Map<HelpRequest, HelpRequestResult>(helpRequest);

        return result;
    }

    public async Task<HelpRequestStatus> ChangeStatusAsync(string id, UpdateHelpRequestStatus request)
    {
        var helpRequestToUpdate = await _helpRequestRepository.GetByIdAsync(id);

        if (helpRequestToUpdate == null)
            throw new NotFoundException(nameof(HelpRequest), id);

        if (helpRequestToUpdate.Status == HelpRequestStatus.Canceled && request.NewStatus == HelpRequestStatus.Closed)
            throw new ValidationException("Canceled request can't be set to closed");
        
        helpRequestToUpdate.Status = request.NewStatus;

        await _helpRequestRepository.UpdateAsync(helpRequestToUpdate);
        _logger.LogInformation("Request #{RequestId} has been updated with new status: {RequestNewStatus}", id, request.NewStatus);
        return request.NewStatus;
    }

    private Expression<Func<HelpRequest, bool>>? CreateFilterPredicate(GetAllHelpRequestsRequest request)
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

        return predicate;
    }
}