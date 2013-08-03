using DevAgenda.Domain.Models;

namespace DevAgenda.Domain.Repositories.Interfaces
{
  // TODO: ME is tagsrepository really needed?
  public interface ITagsRepository : IRepository<Tag>
  {
    Tag FindByName(string tagName);
  }
}
