using Microsoft.AspNetCore.Identity;

namespace Volunteer_Corner.Data.Entities.Identity;

public class UserRole : IdentityUserRole<string> {

    public virtual User User { get; set; }

    public virtual Role Role { get; set; }
}