using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Volunteer_Corner.Business.Exceptions;
using Volunteer_Corner.Business.Infrastructure.Expressions;
using Volunteer_Corner.Business.Interfaces.Services;
using Volunteer_Corner.Business.Models.Requests.HelpSeekers;
using Volunteer_Corner.Business.Models.Results.Abstract;
using Volunteer_Corner.Business.Models.Results.HelpRequests;
using Volunteer_Corner.Business.Models.Results.HelpSeekers;
using Volunteer_Corner.Data.Dtos;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Interfaces;

namespace Volunteer_Corner.Business.Services;

public class HelpSeekerService : IHelpSeekerService
{
    private readonly IMapper _mapper;
    private readonly IHelpSeekerRepository _helpSeekerRepository;
    private readonly IHelpRequestRepository _helpRequestRepository;
    private readonly ILogger<HelpSeekerService> _logger;

    public HelpSeekerService(IMapper mapper, IHelpSeekerRepository helpSeekerRepository,
        ILogger<HelpSeekerService> logger, IHelpRequestRepository helpRequestRepository)
    {
        _mapper = mapper;
        _helpSeekerRepository = helpSeekerRepository;
        _logger = logger;
        _helpRequestRepository = helpRequestRepository;
    }

    public async Task<List<HelpSeekerResult>> GetAllHelpSeekers(GetAllHelpSeekersRequest request)
    {
        var predicate = CreateFilterPredicate(request);

        List<HelpSeeker> source;

        if (predicate == null)
        {
            source = await _helpSeekerRepository.GetAllAsync();
        }
        else
        {
            source = await _helpSeekerRepository.GetAsync(predicate);
        }

        var result = _mapper.Map<List<HelpSeeker>, List<HelpSeekerResult>>(source);
        return result;
    }
    
    public async Task<PageResult<HelpSeekerResult>> GetHelpSeekersPage(GetHelpSeekersPageRequest request)
    {
        var predicate = CreateFilterPredicate(request);
        var source = await _helpSeekerRepository.GetPageAsync(request.PageNumber, request.TakeCount, predicate);

        var result = _mapper.Map<PageDto<HelpSeeker>, PageResult<HelpSeekerResult>>(source);
        return result;
    }

    public async Task<HelpSeekerResult> GetHelpSeekerById(string helpSeekerId)
    {
        var source = await _helpSeekerRepository.GetByIdAsync(helpSeekerId);

        if (source == null)
            throw new NotFoundException(nameof(HelpSeeker), helpSeekerId);
        
        var result = _mapper.Map<HelpSeeker, HelpSeekerResult>(source);
        return result;
    }

    public async Task<bool> ChangeApprovalStatus(string helpSeekerId)
    {
        var helpSeekerToUpdate = await _helpSeekerRepository.GetByIdAsync(helpSeekerId);

        if (helpSeekerToUpdate == null)
            throw new NotFoundException(nameof(HelpSeeker), helpSeekerId);

        helpSeekerToUpdate.IsApproved = !helpSeekerToUpdate.IsApproved;
        await _helpSeekerRepository.UpdateAsync(helpSeekerToUpdate);
        _logger.LogInformation("Status of the help seeker profile #{HelpSeekerId} approval was changed to {NewApprovalStatus}", helpSeekerToUpdate.Id, helpSeekerToUpdate.IsApproved.ToString());
        return helpSeekerToUpdate.IsApproved;
    }

    public async Task<List<HelpRequestResult>> GetOwnHelpRequestsAsync(string helpSeekerId)
    {
        var source = await _helpRequestRepository.GetAsync(x => x.OwnerId == helpSeekerId, disableTracking: false);

        var result = _mapper.Map<List<HelpRequest>, List<HelpRequestResult>>(source);
        return result;
    }
    
    public async Task<HelpRequestWithHelpResponsesResult> GetOwnHelpRequestByIdAsync(string helpSeekerId, string helpRequestId)
    {
        var helpRequest = await _helpRequestRepository.GetByIdWithResponsesAsync(helpRequestId);
        
        if (helpRequest == null)
            throw new NotFoundException(nameof(helpRequest), helpRequestId);

        if (helpRequest.OwnerId != helpSeekerId)
            throw new ValidationException("Owner id and help seeker id don't match");

        var result = _mapper.Map<HelpRequest, HelpRequestWithHelpResponsesResult>(helpRequest);
        return result;
    }

    private static Expression<Func<HelpSeeker, bool>>? CreateFilterPredicate(IGetHelpSeekersRequest request)
    {
        Expression<Func<HelpSeeker, bool>>? predicate = null;

        if (!string.IsNullOrWhiteSpace(request.SearchString))
        {
            Expression<Func<HelpSeeker, bool>> searchStringExpression = x =>
                x.User.FirstName.Contains(request.SearchString)
                || x.User.LastName.Contains(request.SearchString)
                || x.User.Email.Contains(request.SearchString);

            predicate = ExpressionsHelper.And(predicate, searchStringExpression);
        }

        if (request.IsApproved.HasValue)
        {
            Expression<Func<HelpSeeker, bool>> approvalPredicate = x => x.IsApproved == request.IsApproved.Value;
            predicate = ExpressionsHelper.And(predicate, approvalPredicate);
        }

        return predicate;
    }
}