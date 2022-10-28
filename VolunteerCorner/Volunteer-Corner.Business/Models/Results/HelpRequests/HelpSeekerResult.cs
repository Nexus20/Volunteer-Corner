namespace Volunteer_Corner.Business.Models.Results.HelpRequests;

public class HelpSeekerResult
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Patronymic { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
}