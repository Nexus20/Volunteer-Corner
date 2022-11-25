using System.ComponentModel.DataAnnotations;

namespace Volunteer_Corner.Business.Models.Requests.Auth;

public class LoginRequest {
    
    [Required(ErrorMessage = "Email is required.")]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}