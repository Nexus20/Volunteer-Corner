using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volunteer_Corner.Business.Interfaces;
using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results.HelpRequests;

namespace Volunteer_Corner.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class HelpRequestsController : ControllerBase
    {
        private readonly IHelpRequestService _helpRequestService;
        
        // GET: api/HelpRequests
        public HelpRequestsController(IHelpRequestService helpRequestService)
        {
            _helpRequestService = helpRequestService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HelpRequestResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] GetAllHelpRequestsRequest request)
        {
            var result = await _helpRequestService.GetAllHelpRequests(request);
            return Ok(result);
        }

        // GET: api/HelpRequests/5
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(typeof(HelpRequestResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _helpRequestService.GetHelpRequestById(id);
            return Ok(result);
        }

        // POST: api/HelpRequests
        [HttpPost]
        [ProducesResponseType(typeof(HelpRequestResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromForm] CreateHelpRequestRequest request)
        {
            var result = await _helpRequestService.CreateAsync(request, Request.Form.Files, Directory.GetCurrentDirectory());

            return StatusCode(StatusCodes.Status201Created, result);
        }

        // Patch: api/HelpRequests/5
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(HelpRequestResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Patch(int id, [FromBody] string value)
        {
            var result = await _helpRequestService.UpdateAsync(id, value);

            return StatusCode(StatusCodes.Status200OK, result);
        }

        // PUT: api/HelpRequests
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(HelpRequestResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromForm] UpdateHelpRequestRequest request)
        {
            var result = await _helpRequestService.UpdateAsync(request, Request.Form.Files, Directory.GetCurrentDirectory());

            return StatusCode(StatusCodes.Status200OK, result);
        }

        // DELETE: api/HelpRequests/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
