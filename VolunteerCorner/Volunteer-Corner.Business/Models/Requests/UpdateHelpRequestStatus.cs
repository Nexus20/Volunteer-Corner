using System.ComponentModel.DataAnnotations;
using Volunteer_Corner.Business.Validation;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.Business.Models.Requests;

public class UpdateHelpRequestStatus
{
    [Required]
    [ValidateEnum]
    public HelpRequestStatus NewStatus { get; set; }
}