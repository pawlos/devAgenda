using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using DevAgenda.Domain.Models;
using DevAgenda.Domain.Repositories.Interfaces;

namespace DevAgenda.Domain.Repositories
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
            .Include(e => e.Location)
            .Include(e => e.Attendees)
            .Include(e => e.Type);
      }
    }

    public IEnumerable<EventType> AllEventTypes
    {
      get { return _db.EventTypes; }
    }

    // TODO: HI refactor
    public void InsertSearchedArea(SearchedArea searchedArea)
    {
      _db.SearchedAreas.Add(searchedArea);
    }

    public SearchedArea GetSearchedArea(Guid searchedAreaId)
    {
      return
        _db.SearchedAreas
          .SingleOrDefault(sa => sa.Id == searchedAreaId);
    }

    public void AddRSVP(int eventId, int userId)
    {
      //_db.RSVPs.Add(
      //  new RSVP
      //    {
      //      eventId = eventId,
      //      userId = userId
      //    });
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