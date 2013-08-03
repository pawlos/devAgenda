using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace DevAgenda.WebApp.Models
{
  public class EventRepository : IEventRepository
  {
    private readonly DevAgendaCtx _db;

    public EventRepository(DbContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      _db = context as DevAgendaCtx;
    }

    public IQueryable<Event> All
    {
      get
      {
        return 
          _db.Events
            .Include(e => e.Tags)
            .Include(e => e.Type);
      }
    }

    public IQueryable<EventType> AllEventTypes
    {
      get { return _db.EventTypes; }
    }

    public IQueryable<EventDuration> AllEventDurations
    {
      get { return _db.EventDurations; }
    }

    public IQueryable<Country> AllCountries
    {
      get { return _db.Countries; }
    }

    public EventType FindEventTypeById(int eventTypeId)
    {
      return _db.EventTypes
        .SingleOrDefault(et => et.Id == eventTypeId);
    }

    public Event FindById(int id)
    {
      return All
        .Single(e => e.Id == id);
    }

    public void InsertOrUpdate(Event @event)
    {
      if (@event.Id == default(int))
      {
        _db.Events.Add(@event);
      }
      else
      {
        _db.Entry(@event).State = EntityState.Modified;
      }
    }

    public void Save()
    {
      _db.SaveChanges();
    }
  }
}