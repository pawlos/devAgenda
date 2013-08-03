using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DevAgenda.Domain.Models;
using DevAgenda.Domain.Repositories.Interfaces;
using SimpleSocialAuth.MVC3;

namespace DevAgenda.WebApp.Controllers
{
  public class SimpleAuthController : Controller
  {
    private readonly IUserRepository _userRepository;

    public SimpleAuthController(IUserRepository userRepository)
    {
      if (userRepository == null)
      {
        throw new ArgumentNullException("userRepository");
      }

      _userRepository = userRepository;
    }

    public ActionResult LogIn()
    {
      if (Request.IsAuthenticated)
      {
        return
          RedirectToAction("Index", "Events");
      }

      Session["ReturnUrl"] =
        Request.QueryString["returnUrl"];

      return View();
    }

    // TODO: HI Errors handling
    [HttpPost]
    public ActionResult Authenticate(AuthType authType)
    {
      var authHandler =
        AuthHandlerFactory.CreateAuthHandler(authType);

      string redirectUrl =
        authHandler
          .PrepareAuthRequest(
            Request,
            Url.Action("DoAuth", new { authType = (int)authType }));

      return
        Redirect(redirectUrl);
    }

    public ActionResult DoAuth(AuthType authType)
    {
      var authHandler =
        AuthHandlerFactory.CreateAuthHandler(authType);

      var userData = 
        authHandler
          .ProcessAuthRequest(Request as HttpRequestWrapper);

      if (userData == null)
      {
        TempData["authError"] =
          Resources.SimpleAuth.LogIn.AuthenticationFailed;

        return
          RedirectToAction("LogIn");
      }

      var user =
        _userRepository
          .FindByClaimedId(
            userData.UserId,
            (AuthenticatedWith) authType);

      if (user == null)
      {
        user =
          new User
            {
              ClaimedId = userData.UserId,
              AuthenticatedWith = (int) authType,
              UserName = userData.UserName,
              PictureUrl = userData.PictureUrl
            };

        _userRepository
          .InsertOrUpdate(user);

        _userRepository.Save();
      }

      FormsAuthentication.SetAuthCookie(user.Id.ToString(), true);

      return 
        Session["ReturnUrl"] != null
        ? (ActionResult) Redirect((string) Session["ReturnUrl"])
        : RedirectToAction("Index", "Events");
    }
  }
}
