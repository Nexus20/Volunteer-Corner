using System.ComponentModel.DataAnnotations;
using Volunteer_Corner.Business.Models.Enums;
using Volunteer_Corner.Business.Validation;

namespace Volunteer_Corner.Business.Models.Requests;

public class RegisterRequest
{
    [Required]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = null!;

    [Required]
    [Display(Name = "User name")]
    public string UserName { get; set; } = null!;
    
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = null!;
    
    [Required]
    [ValidateEnum]
    public AccountType AccountType { get; set; }
}