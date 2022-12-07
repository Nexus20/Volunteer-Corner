using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volunteer_Corner.Business;
using Volunteer_Corner.Business.Interfaces.Services;
using Volunteer_Corner.Business.Models.Dtos.Files;
using Volunteer_Corner.Business.Models.Requests.HelpRequests;
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
        [Authorize(Roles = CustomRoles.HelpSeekerRole)]
        [ProducesResponseType(typeof(HelpRequestResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromForm] CreateHelpRequestRequest request)
        {
            var helpRequestOwnerId = User.FindFirstValue(CustomClaimTypes.HelpSeekerId);
            
            if (string.IsNullOrWhiteSpace(helpRequestOwnerId))
                return Forbid();

            var result = await _helpRequestService.CreateAsync(request, helpRequestOwnerId, Request.Form.Files, Directory.GetCurrentDirectory());

            return StatusCode(StatusCodes.Status201Created, result);
        }

        // Patch: api/HelpRequests/5/ChangeStatus
        [HttpPatch("{id}/[action]")]
        [Authorize(Roles = CustomRoles.HelpSeekerRole)]
        [ProducesResponseType(typeof(HelpRequestResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeStatus(string id, [FromBody] UpdateHelpRequestStatus request)
        {
            var result = await _helpRequestService.ChangeStatusAsync(id, request);

            return StatusCode(StatusCodes.Status200OK, result);
        }

        // PUT: api/HelpRequests
        [HttpPut("{id}")]
        [Authorize(Roles = CustomRoles.HelpSeekerRole)]
        [ProducesResponseType(typeof(HelpRequestResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateHelpRequestRequest request)
        {
            var result = await _helpRequestService.UpdateAsync(id, request);
            return StatusCode(StatusCodes.Status200OK, result);
        }
        
        [HttpPatch("{id}/[action]")]
        public async Task<IActionResult> DeleteDocuments(string id, [FromBody] DeleteHelpRequestDocumentsRequest request)
        {
            await _helpRequestService.DeleteDocumentsAsync(id, request);
            return NoContent();
        }
        
        [HttpPatch("{id}/[action]")]
        public async Task<IActionResult> AddDocuments(string id)
        {
            if (!Request.Form.Files.Any())
                return BadRequest();
            
            var filesDtos = new List<FileDto>();

            foreach (var formFile in Request.Form.Files)
            {
                filesDtos.Add(new FileDto()
                {
                    Content = formFile.OpenReadStream(),
                    Name = formFile.FileName,
                    ContentType = formFile.ContentType
                });
            }
            
            var result = await _helpRequestService.AddDocumentsAsync(id, filesDtos);
            return Ok(result);
        }

        // DELETE: api/HelpRequests/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
