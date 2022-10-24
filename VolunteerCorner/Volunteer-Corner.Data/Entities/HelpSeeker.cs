using Volunteer_Corner.Data.Entities.Abstract;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.Data.Entities;

public class HelpSeeker : BaseEntity
{
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
    public List<HelpRequest>? HelpRequests { get; set; }
}