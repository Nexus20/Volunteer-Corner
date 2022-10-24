using Volunteer_Corner.Data.Entities.Abstract;

namespace Volunteer_Corner.Data.Entities.Identity;

public class UserDocument : BaseEntity
{
    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
    public string FilePath { get; set; } = null!;
}