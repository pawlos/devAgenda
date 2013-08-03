using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DevAgenda.Domain.Models;

namespace DevAgenda.WebApp.Services
{
  public class LocationsQueryCache : ILocationsQueryCache
  {
    private readonly DevAgendaCtx _db;

    public LocationsQueryCache(DbContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      _db = context as DevAgendaCtx;
    }

    private IQueryable<LocationsQueryResult> All
    {
      get
      {
        return
          _db.LocationsQueryResults
            .Include(lqr => lqr.Location);
      }
    }

    public IEnumerable<Location> Get(string locationsQuery)
    {
      if (!All.Any(lqr => lqr.Query == locationsQuery))
      {
        return null;
      }

      return
        All
          .Where(lqr => lqr.Query == locationsQuery && lqr.LocationId != null)
          .Select(lqr => lqr.Location);
    }

    public void Set(string locationsQuery, IEnumerable<Location> locations)
    {
      if (locations == null)
      {
        throw new ArgumentNullException("locations");
      }

      foreach (var location in locations)
      {
        _db.LocationsQueryResults
          .Add(
            new LocationsQueryResult
              {
                Query = locationsQuery,
                LocationId = location.Id
              });
      }

      _db.SaveChanges();
    }
  }
}
