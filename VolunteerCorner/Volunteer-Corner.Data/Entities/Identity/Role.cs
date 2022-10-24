using Microsoft.AspNetCore.Identity;

namespace Volunteer_Corner.Data.Entities.Identity;

public class Role : IdentityRole
{
    public List<UserRole> UserRoles { get; set; }
}