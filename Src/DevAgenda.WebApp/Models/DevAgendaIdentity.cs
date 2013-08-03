using System;
using System.Security.Principal;
using DevAgenda.Domain.Models;

namespace DevAgenda.WebApp.Models
{
  // TODO: HI Clean up
  public class DevAgendaIdentity : IIdentity
  {
    private readonly User _user;

    public string Name
    {
      get { return _user.Id.ToString(); }
    }

    public string AuthenticationType
    {
      get { return "devAgenda"; }
    }

    public bool IsAuthenticated
    {
      get { return true; }
    }

    public string UserName
    {
      get
      {
        return _user.UserName;
      }
    }

    public int UserId
    {
      get
      {
        return _user.Id;
      }
    }

    public DevAgendaIdentity(User user)
    {
      if (user == null)
      {
        throw new ArgumentNullException("user");
      }

      _user = user;
    }
  }
}