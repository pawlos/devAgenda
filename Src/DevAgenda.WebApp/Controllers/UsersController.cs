using System;
using System.Web.Mvc;
using System.Web.Security;
using DevAgenda.Domain.Repositories.Interfaces;
using Elmah.Contrib.Mvc;

namespace DevAgenda.WebApp.Controllers
{
  [ElmahHandleError]
  public class UsersController : Controller
  {
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
      if (userRepository == null)
      {
        throw new ArgumentNullException("userRepository");
      }

      _userRepository = userRepository;
    }

    // TODO: ME what about logging off of providers?
    public ActionResult LogOut()
    {
      FormsAuthentication.SignOut();

      return RedirectToAction("Index", "Events");
    }

    public ActionResult Profile(int userId, string userName)
    {
      var user =
        _userRepository
          .FindById(userId);

      return 
        View(user);
    }

  }
}
