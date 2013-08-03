using System;
using System.Data.Entity;
using System.Linq;

namespace DevAgenda.WebApp.Models
{
  public class TagsRepository : ITagsRepository
  {
    private readonly DevAgendaCtx _db;

    public TagsRepository(DbContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException("context");
      }

      _db = context as DevAgendaCtx;
    }

    public IQueryable<Tag> All
    {
      get { return _db.Tags; }
    }

    public Tag FindById(int id)
    {
      return All
        .SingleOrDefault(t => t.Id == id);
    }

    public void InsertOrUpdate(Tag entity)
    {
      throw new System.NotImplementedException();
    }

    public void Save()
    {
      throw new System.NotImplementedException();
    }

    public Tag FindByName(string tagName)
    {
      return All
        .SingleOrDefault(t => t.Name == tagName);
    }
  }
}