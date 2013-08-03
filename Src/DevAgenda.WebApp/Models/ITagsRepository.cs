namespace DevAgenda.WebApp.Models
{
  public interface ITagsRepository : IRepository<Tag>
  {
    Tag FindByName(string tagName);
  }
}
