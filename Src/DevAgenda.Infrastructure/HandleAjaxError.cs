using System;
using System.Reflection;
using System.Web.Mvc;
using NLog;

namespace DevAgenda.Infrastructure
{
  public class HandleAjaxError : FilterAttribute, IExceptionFilter
  {
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public string ErrorMessageResourceName { get; set; }

    public Type ErrorMessageResourceType { get; set; }

    public void OnException(ExceptionContext filterContext)
    {
      if (!filterContext.HttpContext.Request.IsAjaxRequest())
      {
        return;
      }

      if (_logger.IsErrorEnabled)
      {
        _logger.ErrorException("Error while ajax request.", filterContext.Exception);
      }

      filterContext.ExceptionHandled = true;

      string errorMessage = "An error has occured while ajax request.";
      string errorMessageResourceName = "GenericAjaxError";
      Type errorMessageResourceType = typeof (Resources.Common);

      if (!string.IsNullOrEmpty(ErrorMessageResourceName) && ErrorMessageResourceType != null)
      {
        errorMessageResourceName = ErrorMessageResourceName;
        errorMessageResourceType = ErrorMessageResourceType;
      }

      var property =
        errorMessageResourceType
          .GetProperty(
            errorMessageResourceName,
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

      if (property != null && property.PropertyType == typeof(string))
      {
        errorMessage =
          (string)property
            .GetValue(null, null);
      }

      filterContext.Result =
          new JsonResult
          {
            Data = new { errorMessage }
          };
    }
  }
}
