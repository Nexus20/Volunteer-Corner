using Microsoft.AspNetCore.Identity;

namespace Volunteer_Corner.Data.Entities.Identity;

public class UserRole : IdentityUserRole<string> {

    public User User { get; set; }

    public Role Role { get; set; }
}