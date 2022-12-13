using System.ComponentModel.DataAnnotations;

namespace Volunteer_Corner.Business.Models.Requests.Base;

public abstract class GetPageRequest
{
    [Required] public int TakeCount { get; set; }
    [Required] public int PageNumber { get; set; }
}