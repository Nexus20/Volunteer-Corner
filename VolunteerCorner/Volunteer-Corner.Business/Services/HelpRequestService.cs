using System.Linq.Expressions;
using AutoMapper;
using Volunteer_Corner.Business.Infrastructure.Expressions;
using Volunteer_Corner.Business.Interfaces;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results.HelpRequests;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Interfaces;

namespace Volunteer_Corner.Business.Services;

public class HelpRequestService : IHelpRequestService
{
    private readonly IMapper _mapper;
    private readonly IRepository<HelpRequest> _helpRequestRepository;

    public HelpRequestService(IRepository<HelpRequest> helpRequestRepository, IMapper mapper)
    {
        _helpRequestRepository = helpRequestRepository;
        _mapper = mapper;
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

        var result = _mapper.Map<HelpRequest, HelpRequestResult>(source);
        return result;
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