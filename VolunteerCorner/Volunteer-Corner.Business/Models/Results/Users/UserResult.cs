using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Business.Models.Results.Users;

public class UserResult
{
    public string Id { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public ContactsDisplayPolicy ContactsDisplayPolicy { get; set; }
}