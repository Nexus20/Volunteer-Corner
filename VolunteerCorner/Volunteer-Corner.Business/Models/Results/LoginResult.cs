namespace Volunteer_Corner.Business.Models.Results;

public class LoginResult
{
    public bool IsAuthSuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Token { get; set; }
}