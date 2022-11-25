using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Requests.Auth;
using Volunteer_Corner.Business.Models.Requests.Users;
using Volunteer_Corner.Business.Models.Results;

namespace Volunteer_Corner.Business.Interfaces;

public interface IUserService
{
    Task<UserResult> RegisterAsync(RegisterRequest request);
    Task<UserResult> UpdateOwnProfileAsync(string profileOwnerId, UpdateOwnProfileRequest updateProfileRequest);
}