using Volunteer_Corner.Business.Models.Results.Abstract;

namespace Volunteer_Corner.Business.Models.Results.HelpRequests;

public class HelpSeekerResult : BaseResult
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Patronymic { get; set; }
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public bool IsApproved { get; set; }
}