using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results;

namespace Volunteer_Corner.Business.Interfaces;

public interface ISignInService {

    Task<LoginResult> SignInAsync(LoginRequest request);
}