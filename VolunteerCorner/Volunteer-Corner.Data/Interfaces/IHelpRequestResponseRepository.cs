using Volunteer_Corner.Data.Entities;

namespace Volunteer_Corner.Data.Interfaces;

public interface IHelpRequestResponseRepository : IRepository<HelpRequestResponse>
{
    Task<HelpRequestResponse?> GetByIdWithDetailsAsync(string id);
}