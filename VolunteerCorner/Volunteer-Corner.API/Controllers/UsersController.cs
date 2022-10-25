using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volunteer_Corner.Business.Interfaces;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results;

namespace Volunteer_Corner.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RegisterResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _userService.RegisterAsync(request);
            return StatusCode(StatusCodes.Status201Created, result);
        }
    }
}
