using System;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace DevAgenda.WebApp.Models
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

    public void Save()
    {
      _db.SaveChanges();
    }

    public User FindByClaimedId(string claimedIdentifier)
    {
      return All
        .SingleOrDefault(u => u.ClaimedId == claimedIdentifier);
    }
  }
}