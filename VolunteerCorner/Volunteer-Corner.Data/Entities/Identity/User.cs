using Microsoft.AspNetCore.Identity;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Data.Entities.Identity;

public class User : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Patronymic { get; set; }
    public virtual List<UserRole> UserRoles { get; set; }
    
    public virtual HelpSeeker? HelpSeeker { get; set; }
    public virtual Volunteer? Volunteer { get; set; }

    public virtual List<UserDocument>? Documents { get; set; }
    
    public ContactsDisplayPolicy ContactsDisplayPolicy { get; set; }

    public User()
    {
        ContactsDisplayPolicy = ContactsDisplayPolicy.AllowAll;
    }
}