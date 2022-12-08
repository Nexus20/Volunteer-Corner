using AutoMapper;
using Microsoft.Extensions.Logging;
using Volunteer_Corner.Business.Interfaces.Services;
using Volunteer_Corner.Business.Models.Requests.HelpSeekers;
using Volunteer_Corner.Business.Models.Results.HelpRequests;
using Volunteer_Corner.Data.Interfaces;

namespace Volunteer_Corner.Business.Services;

public class HelpSeekerService : IHelpSeekerService
{
    private readonly IMapper _mapper;
    private readonly IHelpSeekerRepository _helpSeekerRepository;
    private readonly ILogger<HelpSeekerService> _logger;

    public HelpSeekerService(IMapper mapper, IHelpSeekerRepository helpSeekerRepository,
        ILogger<HelpSeekerService> logger)
    {
        _mapper = mapper;
        _helpSeekerRepository = helpSeekerRepository;
        _logger = logger;
    }

    public Task<List<HelpSeekerResult>> GetAllHelpSeekers(GetAllHelpSeekersRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<HelpSeekerResult> GetHelpSeekerById(string helpSeekerId)
    {
        throw new NotImplementedException();
    }
}