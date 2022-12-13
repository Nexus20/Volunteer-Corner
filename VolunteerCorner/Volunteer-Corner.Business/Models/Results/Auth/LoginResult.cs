using Volunteer_Corner.Business.Models.Results.Users;

namespace Volunteer_Corner.Business.Models.Results.Auth;

public class LoginResult
{
    public bool IsAuthSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Token { get; set; }
    
    public UserResult? UserInfo { get; set; }
}