using System;
using System.IO;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using HtmlTags;

namespace DevAgenda.WebApp.Helpers
{
  public static class HtmlExtensions
  {
    // TODO: ME reflection to get [Required] tag from property
    public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, bool isRequired)
    {
      var tag = 
        new TagBuilder("div")
          {
            InnerHtml = html.LabelFor(expression).ToHtmlString()
          };

      tag.Attributes.Add("style", "overflow: hidden; width: 100%");

      if (isRequired)
      {
        var requiredTag = new TagBuilder("span");
        requiredTag.AddCssClass("required-value");
        requiredTag.SetInnerText("*");

        tag.InnerHtml += requiredTag.ToString();
      }

      return 
        new MvcHtmlString(tag.ToString());
    }

    public static MvcHtmlString IncludeCreateEventScripts(this HtmlHelper html)
    {
      var userLanguages =
        html.ViewContext.HttpContext.Request.UserLanguages;

      string locale = "en";

      if (userLanguages != null && userLanguages.Length > 0)
      {
        locale =
          userLanguages[0].Split('-')[0];
      }

      string filePath =
        html.ViewContext.HttpContext.Request.MapPath(@"~/Scripts/createEvent-" + locale + ".js");

      var localesScriptTag = 
        new HtmlTag("script")
          .Attr("type", "text/javascript")
          .Attr("src", File.Exists(filePath)
                        ? UrlHelper.GenerateContentUrl(@"~/Scripts/createEvent-" + locale + ".js", html.ViewContext.HttpContext)
                        : UrlHelper.GenerateContentUrl(@"~/Scripts/createEvent-en.js", html.ViewContext.HttpContext));

      var createEventScriptTag =
        new HtmlTag("script")
          .Attr("type", "text/javascript")
          .Attr("src", UrlHelper.GenerateContentUrl(@"~/Scripts/createEvent.js", html.ViewContext.HttpContext));

      return
        new MvcHtmlString(
          localesScriptTag
            .After(createEventScriptTag)
            .ToHtmlString());
    }
  }
}