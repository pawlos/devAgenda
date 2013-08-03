using System.Linq;

namespace DevAgenda.WebApp.Models
{
  public interface IEventRepository : IRepository<Event>
  {
    IQueryable<EventType> AllEventTypes { get; }
    IQueryable<EventDuration> AllEventDurations { get; }
    IQueryable<Country> AllCountries { get; }
    EventType FindEventTypeById(int eventTypeId);
  }
}
