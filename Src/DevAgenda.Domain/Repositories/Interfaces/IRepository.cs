using System.Linq;

namespace DevAgenda.Domain.Repositories.Interfaces
{
  public interface IRepository<T>
  {
    IQueryable<T> All { get; }
    T FindById(int id);
    void InsertOrUpdate(T entity);
    void Save();
  }
}