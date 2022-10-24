using Volunteer_Corner.Data.Entities.Abstract;

namespace Volunteer_Corner.Data.Entities;

public class HelpRequestDocument : BaseEntity
{
    public string HelpRequestId { get; set; } = null!;
    public HelpRequest HelpRequest { get; set; } = null!;
    public string FilePath { get; set; } = null!;
}