using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volunteer_Corner.Business;
using Volunteer_Corner.Business.Interfaces.Services;
using Volunteer_Corner.Business.Models.Requests.Auth;
using Volunteer_Corner.Business.Models.Requests.HelpRequests;
using Volunteer_Corner.Business.Models.Requests.HelpSeekers;
using Volunteer_Corner.Business.Models.Requests.Users;
using Volunteer_Corner.Business.Models.Results;
using Volunteer_Corner.Business.Models.Results.HelpRequests;

namespace Volunteer_Corner.API.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class HelpSeekersController : ControllerBase
{
    private readonly IHelpSeekerService _helpSeekerService;
    private readonly IHelpRequestService _helpRequestService;

    public HelpSeekersController(IHelpSeekerService helpSeekerService, IHelpRequestService helpRequestService)
    {
        _helpSeekerService = helpSeekerService;
        _helpRequestService = helpRequestService;
    }
    
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<HelpSeekerResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] GetAllHelpSeekersRequest request)
    {
        var result = await _helpSeekerService.GetAllHelpSeekers(request);
        return Ok(result);
    }

    // GET: api/HelpSeekers/5
    [HttpGet("{id}", Name = "Get help seeker by id")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(HelpSeekerResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _helpSeekerService.GetHelpSeekerById(id);
        return Ok(result);
    }

    [HttpPatch("{helpSeekerId}/[action]")]
    [Authorize(Roles = $"{CustomRoles.AdminRole},{CustomRoles.SuperAdminRole}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeApprovalStatus(string helpSeekerId)
    {
        var result = await _helpSeekerService.ChangeApprovalStatus(helpSeekerId);
        return Ok(result);
    }

    [HttpGet("[action]", Name = "Get all help seeker's requests")]
    [ProducesResponseType(typeof(List<HelpRequestResult>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOwnHelpRequests()
    {
        var helpSeekerId = User.FindFirstValue(CustomClaimTypes.HelpSeekerId);

        if (string.IsNullOrWhiteSpace(helpSeekerId))
            return Forbid();
            
        var result = await _helpRequestService.GetAllHelpRequests(new GetAllHelpRequestsRequest()
        {
            OwnerId = helpSeekerId
        });
        return Ok(result);
    }
}