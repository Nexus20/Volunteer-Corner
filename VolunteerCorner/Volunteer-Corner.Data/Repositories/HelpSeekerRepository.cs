using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Interfaces;

namespace Volunteer_Corner.Data.Repositories;

public class HelpSeekerRepository : Repository<HelpSeeker>, IHelpSeekerRepository
{
    public HelpSeekerRepository(ApplicationDbContext db) : base(db)
    {
    }
}