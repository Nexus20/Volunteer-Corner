using Microsoft.AspNetCore.Identity;

namespace Volunteer_Corner.Data.Entities.Identity;

public class User : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
    public List<UserRole> UserRoles { get; set; }
    
    public HelpSeeker? HelpSeeker { get; set; }
    public Volunteer? Volunteer { get; set; }

    public List<UserDocument>? Documents { get; set; }
}