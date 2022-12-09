using System.ComponentModel.DataAnnotations;
using Volunteer_Corner.Business.Validation;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Business.Models.Requests.Users;

public class AdjustContactsDisplaying
{
    [Required]
    [ValidateEnum]
    public ContactsDisplayPolicy NewPolicy { get; set; }
}