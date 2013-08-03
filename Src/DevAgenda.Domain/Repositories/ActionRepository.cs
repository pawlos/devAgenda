using System;
using System.Data.Entity;
using System.Linq;
using DevAgenda.Domain.Models;
using DevAgenda.Domain.Repositories.Interfaces;
using Action = DevAgenda.Domain.Models.Action;

namespace DevAgenda.Domain.Repositories
{
  public class ActionRepository : IActionRepository
  {
    private readonly DevAgendaCtx _db;

    public ActionRepository(DbContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      _db = context as DevAgendaCtx;
    }

    public IQueryable<Action> All
    {
      get
      {
        return
          _db.Actions
            .Include(a => a.Event)
            .Include(a => a.Event.CreatedBy)
            .Include(a => a.User);
      }
    }

    public Action FindById(int id)
    {
      return All
        .SingleOrDefault(a => a.Id == id);
    }

    public void InsertOrUpdate(Action action)
    {
      if (action.Id == default(int))
      {
        _db.Actions.Add(action);
      }
      else
      {
        throw new NotSupportedException("Action editing not supported.");
      }
    }

    public void Save()
    {
      _db.SaveChanges();
    }
  }
}
