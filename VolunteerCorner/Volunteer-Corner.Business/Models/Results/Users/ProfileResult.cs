using Volunteer_Corner.Business.Models.Results.HelpSeekers;
using Volunteer_Corner.Business.Models.Results.Volunteers;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Business.Models.Results.Users;

public class ProfileResult
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Patronymic { get; set; }
    public virtual List<RoleResult> Roles { get; set; }
    
    public virtual HelpSeekerResult? HelpSeeker { get; set; }
    public virtual VolunteerResult? Volunteer { get; set; }

    public ContactsDisplayPolicy ContactsDisplayPolicy { get; set; }
}