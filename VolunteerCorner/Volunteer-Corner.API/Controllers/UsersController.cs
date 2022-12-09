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
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ISignInService _signInService;

    public UsersController(IUserService userService, ISignInService signInService)
    {
        _userService = userService;
        _signInService = signInService;
    }

    [HttpPost("[action]")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResult), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _userService.RegisterAsync(request);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("[action]")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _signInService.SignInAsync(request);
        return result.IsAuthSuccessful ? Ok(result) : Unauthorized(result);
    }

    [HttpPut("[action]")]
    [ProducesResponseType(typeof(UserResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateOwnProfileRequest updateOwnProfileRequest)
    {
        var profileOwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(profileOwnerId))
            return Forbid();
            
        var result = await _userService.UpdateOwnProfileAsync(profileOwnerId, updateOwnProfileRequest);
        return Ok(result);
    }

    [HttpPatch("[action]")]
    public async Task<IActionResult> AdjustContactsDisplaying([FromBody] AdjustContactsDisplaying request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();
        
        var result = await _userService.AdjustContactsDisplaying(userId, request);
        return Ok(result);
    }
}