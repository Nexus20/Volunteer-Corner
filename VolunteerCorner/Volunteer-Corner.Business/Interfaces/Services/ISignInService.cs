using Volunteer_Corner.Business.Models.Requests.Auth;
using Volunteer_Corner.Business.Models.Results.Auth;

namespace Volunteer_Corner.Business.Interfaces.Services;

public interface ISignInService {

    Task<LoginResult> SignInAsync(LoginRequest request);
}