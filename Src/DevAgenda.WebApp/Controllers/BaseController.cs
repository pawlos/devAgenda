using System.Collections.Generic;
using System.Web.Mvc;
using DevAgenda.Domain;

namespace DevAgenda.WebApp.Controllers
{
  public class BaseController : Controller
  {
    public BaseController()
    {
      DomainEvents.Container = new DependencyResolverContainer();
    }
  }

  public class DependencyResolverContainer : IContainer
  {
    public IEnumerable<T> ResolveAll<T>()
    {
      return DependencyResolver.Current.GetServices<T>();
    }
  }
}