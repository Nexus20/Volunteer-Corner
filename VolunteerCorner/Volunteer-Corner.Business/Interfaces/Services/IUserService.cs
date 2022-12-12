using Volunteer_Corner.Business.Models.Requests.Auth;
using Volunteer_Corner.Business.Models.Requests.Users;
using Volunteer_Corner.Business.Models.Results.Users;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Business.Interfaces.Services;

public interface IUserService
{
    Task<UserResult> RegisterAsync(RegisterRequest request);
    Task<UserResult> UpdateOwnProfileAsync(string profileOwnerId, UpdateOwnProfileRequest updateProfileRequest);
    Task<ContactsDisplayPolicy> AdjustContactsDisplaying(string userId, AdjustContactsDisplaying request);
    Task<ProfileResult> GetProfileAsync(string userId);
}