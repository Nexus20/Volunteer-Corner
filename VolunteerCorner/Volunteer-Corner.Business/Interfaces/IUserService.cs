using Volunteer_Corner.Business.Models.Requests;
using Volunteer_Corner.Business.Models.Results;

namespace Volunteer_Corner.Business.Interfaces;

public interface IUserService
{
    Task<RegisterResult> RegisterAsync(RegisterRequest request);
    Task<RegisterResult> EditAsync(RegisterRequest request);
}