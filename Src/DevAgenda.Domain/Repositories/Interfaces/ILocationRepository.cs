using DevAgenda.Domain.Models;

namespace DevAgenda.Domain.Repositories.Interfaces
{
  public interface ILocationRepository : IRepository<Location>
  {
    Location FindByCoords(decimal latitude, decimal longitude);
  }
}
