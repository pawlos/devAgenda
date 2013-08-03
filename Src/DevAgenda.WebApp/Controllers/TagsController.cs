using System;
using System.Linq;
using System.Web.Mvc;
using DevAgenda.Domain.Repositories.Interfaces;

namespace DevAgenda.WebApp.Controllers
{
  public class TagsController : Controller
  {
    private readonly ITagsRepository _tagsRepository;

    public TagsController(ITagsRepository tagsRepository)
    {
      if (tagsRepository == null)
      {
        throw new ArgumentNullException("tagsRepository");
      }

      _tagsRepository = tagsRepository;
    }

    [Authorize]
    public ActionResult Search(string term)
    {
      if (string.IsNullOrEmpty(term))
      {
        return Json(new string[] {}, JsonRequestBehavior.AllowGet);
      }

      var tags =
        _tagsRepository
          .All
          .Where(t => t.Name.Contains(term))
          .Select(t => t.Name)
          .ToArray();

      return Json(tags, JsonRequestBehavior.AllowGet);
    }
  }
}
