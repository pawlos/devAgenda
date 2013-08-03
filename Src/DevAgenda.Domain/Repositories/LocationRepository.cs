using System;
using System.Data.Entity;
using System.Linq;
using DevAgenda.Domain.Models;
using DevAgenda.Domain.Repositories.Interfaces;

namespace DevAgenda.Domain.Repositories
{
  public class LocationRepository : ILocationRepository
  {
    private readonly DevAgendaCtx _db;

    public LocationRepository(DbContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      _db = context as DevAgendaCtx;
    }

    public IQueryable<Location> All
    {
      get { return _db.Locations; }
    }

    public Location FindById(int id)
    {
      return
        All
          .SingleOrDefault(l => l.Id == id);
    }

    public void InsertOrUpdate(Location location)
    {
      if (location.Id == default(int))
      {
        _db.Locations.Add(location);
      }
      else
      {
        throw new NotSupportedException("Updating Locations is not supported");
      }
    }

    public void Save()
    {
      _db.SaveChanges();
    }

    public Location FindByCoords(decimal latitude, decimal longitude)
    {
      return
        All
          .SingleOrDefault(l => l.Latitude == latitude && l.Longitude == longitude);
    }
  }
}
