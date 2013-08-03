using System.Collections.Generic;
using DevAgenda.Domain.Models;

namespace DevAgenda.WebApp.Services
{
  public interface ILocationsQueryCache
  {
    IEnumerable<Location> Get(string locationsQuery);
    void Set(string locationsQuery, IEnumerable<Location> locations);
  }
}
