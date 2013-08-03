using System.Linq;
using DevAgenda.Domain.Models;

namespace DevAgenda.Domain.Repositories.Interfaces
{
  public interface IUserRepository : IRepository<User>
  {
    User FindByClaimedId(string claimedIdentifier, AuthenticatedWith authenticatedWith);
    IQueryable<EventsFilter> AllEventsFilters { get; }
    EventsFilter GetUserFilter(int userId);

    void InsertOrUpdateEventsFilter(EventsFilter eventsFilter);
  }
}