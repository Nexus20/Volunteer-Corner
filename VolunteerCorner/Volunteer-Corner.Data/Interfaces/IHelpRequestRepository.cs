using System.Linq.Expressions;
using Volunteer_Corner.Data.Entities;

namespace Volunteer_Corner.Data.Interfaces;

public interface IHelpRequestRepository : IRepository<HelpRequest>
{
    public Task<List<HelpRequest>> GetAsync(Expression<Func<HelpRequest, bool>>? predicate = null,
        Func<IQueryable<HelpRequest>, IOrderedQueryable<HelpRequest>> orderBy = null,
        bool disableTracking = true);
    public Task DeleteDocumentsAsync(List<HelpRequestDocument> documentsToRemove);
    public Task AddDocumentsAsync(List<HelpRequestDocument> documentsToAdd);
}