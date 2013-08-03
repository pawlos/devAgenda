namespace DevAgenda.WebApp.Models
{
  public interface IUserRepository : IRepository<User>
  {
    User FindByClaimedId(string claimedIdentifier);
  }
}