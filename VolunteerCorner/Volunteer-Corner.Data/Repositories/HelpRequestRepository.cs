using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Interfaces;

namespace Volunteer_Corner.Data.Repositories;

public class HelpRequestRepository : Repository<HelpRequest>, IHelpRequestRepository
{
    private readonly ILogger<HelpRequestRepository> _logger;

    public HelpRequestRepository(ApplicationDbContext db, ILogger<HelpRequestRepository> logger) : base(db)
    {
        _logger = logger;
    }

    public override async Task<HelpRequest> AddAsync(HelpRequest entity)
    {
        await using var transaction = await Db.Database.BeginTransactionAsync();
        try
        {
            await Db.AddAsync(entity);
            await Db.SaveChangesAsync();
            await transaction.CommitAsync();

            return entity;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            _logger.LogError("Error while creating a new help request: {EMessage}", e.Message);
        }

        return null;
    }

    public Task<List<HelpRequest>> GetAsync(Expression<Func<HelpRequest, bool>>? predicate = null, Func<IQueryable<HelpRequest>, IOrderedQueryable<HelpRequest>> orderBy = null, bool disableTracking = true)
    {
        var query = Db.HelpRequests
            .Include(x => x.AdditionalDocuments)
            .Include(x => x.Owner)
            .ThenInclude(x => x.User)
            .AsQueryable();

        if (predicate != null)
            query = query.Where(predicate);

        if (disableTracking)
            query = query.AsNoTracking();

        if (orderBy != null)
            query = orderBy(query);

        return query.ToListAsync();
    }

    public Task DeleteDocumentsAsync(List<HelpRequestDocument> documentsToRemove)
    {
        Db.HelpRequestDocuments.RemoveRange(documentsToRemove);
        return Db.SaveChangesAsync();
    }

    public async Task AddDocumentsAsync(List<HelpRequestDocument> documentsToAdd)
    {
        await Db.HelpRequestDocuments.AddRangeAsync(documentsToAdd);
        await Db.SaveChangesAsync();
    }

    public Task<HelpRequest?> GetByIdWithResponsesAsync(string id)
    {
        return Db.HelpRequests
            .Include(x => x.Owner)
            .ThenInclude(x => x.User)
            .Include(x => x.AdditionalDocuments)
            .Include(x => x.Responses)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public override Task<HelpRequest?> GetByIdAsync(string id)
    {
        return Db.HelpRequests.AsNoTracking()
            .Include(x => x.AdditionalDocuments)
            .Include(x => x.Owner)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}