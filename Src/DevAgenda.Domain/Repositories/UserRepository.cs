using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using DevAgenda.Domain.Models;
using DevAgenda.Domain.Repositories.Interfaces;

namespace DevAgenda.Domain.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly DevAgendaCtx _db;

    public UserRepository(DbContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      _db = context as DevAgendaCtx;
    }

    public IQueryable<User> All
    {
      get { return _db.Users; }
    }

    public IQueryable<EventsFilter> AllEventsFilters
    {
      get { return _db.EventsFilters; }
    }

    public User FindById(int id)
    {
      return All
        .SingleOrDefault(u => u.Id == id);
    }

    public void InsertOrUpdate(User user)
    {
      if (user.Id == default(int))
      {
        _db.Users.Add(user);
      }
      else
      {
        _db.Entry(user).State = EntityState.Modified;
      }
    }

    public void InsertOrUpdateEventsFilter(EventsFilter eventsFilter)
    {
      if (eventsFilter.Id == default(int))
      {
        _db.EventsFilters.Add(eventsFilter);
      } 
      else
      {
        _db.Entry(eventsFilter).State = EntityState.Modified;
      }
    }

    public void Save()
    {
      _db.SaveChanges();
    }

    public User FindByClaimedId(string claimedIdentifier, AuthenticatedWith authenticatedWith)
    {
      return All
        .SingleOrDefault(u => u.ClaimedId == claimedIdentifier && u.AuthenticatedWith == (int)authenticatedWith);
    }

    public EventsFilter GetUserFilter(int userId)
    {
      return
        AllEventsFilters
          .Include(ef => ef.Location)
          .SingleOrDefault(ef => ef.UserId == userId);
    }
  }
}