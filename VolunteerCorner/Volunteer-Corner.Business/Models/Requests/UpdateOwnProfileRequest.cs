using System.ComponentModel.DataAnnotations;

namespace Volunteer_Corner.Business.Models.Requests
{
    public class UpdateOwnProfileRequest
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
    }
}
