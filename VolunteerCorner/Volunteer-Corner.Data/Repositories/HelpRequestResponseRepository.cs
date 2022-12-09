using Microsoft.EntityFrameworkCore;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Interfaces;

namespace Volunteer_Corner.Data.Repositories;

public class HelpRequestResponseRepository : Repository<HelpRequestResponse>, IHelpRequestResponseRepository
{
    public HelpRequestResponseRepository(ApplicationDbContext db) : base(db)
    {
    }

    public async Task<HelpRequestResponse?> GetByIdWithDetailsAsync(string id)
    {
        var helpRequestResponse = await Db.HelpRequestResponses
            .Include(x => x.VolunteerFrom)
            .ThenInclude(x => x.User)
            .Include(x => x.HelpRequestTo)
            .ThenInclude(x => x.Owner)
            .ThenInclude(x => x.User)
            .Include(x => x.HelpRequestTo)
            .ThenInclude(x => x.AdditionalDocuments)
            .FirstOrDefaultAsync(x => x.Id == id);

        // if (helpRequestResponse == null)
        //     return null;
        //
        // if (!string.IsNullOrWhiteSpace(helpRequestResponse.IncludedHelpProposalId))
        //     helpRequestResponse.IncludedHelpProposal = await Db.HelpProposals
        //         .AsNoTracking()
        //         .Include(x => x.Owner)
        //         .ThenInclude(x => x.User)
        //         .FirstAsync(x => x.Id == helpRequestResponse.IncludedHelpProposalId);
        //
        // Db.ChangeTracker.LazyLoadingEnabled = true;
        return helpRequestResponse;
    }
}