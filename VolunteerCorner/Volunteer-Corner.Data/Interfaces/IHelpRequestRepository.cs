using Volunteer_Corner.Data.Entities;

namespace Volunteer_Corner.Data.Interfaces;

public interface IHelpRequestRepository : IRepository<HelpRequest>
{
    public Task DeleteDocumentsAsync(List<HelpRequestDocument> documentsToRemove);
    public Task AddDocumentsAsync(List<HelpRequestDocument> documentsToAdd);
}