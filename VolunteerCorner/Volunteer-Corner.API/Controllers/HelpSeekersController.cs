using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volunteer_Corner.Business;
using Volunteer_Corner.Business.Interfaces.Services;
using Volunteer_Corner.Business.Models.Requests.Auth;
using Volunteer_Corner.Business.Models.Requests.HelpRequests;
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