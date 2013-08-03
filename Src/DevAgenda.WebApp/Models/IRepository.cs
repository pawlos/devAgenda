using System.Linq;

namespace DevAgenda.WebApp.Models
{
  public interface IRepository<T>
  {
    IQueryable<T> All { get; }
    T FindById(int id);
    void InsertOrUpdate(T entity);
    void Save();
  }
}