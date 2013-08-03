using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DevAgenda.Domain.Models;
using DevAgenda.Domain.Repositories.Interfaces;
using DevAgenda.WebApp.Models;
using NLog;

namespace DevAgenda.WebApp
{
  public class MvcApplication : HttpApplication
  {
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
    }

    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


      routes.MapRoute(
        "AddNewEvent",
        "Events/Add",
        new {controller = "Events", action = "Create"});

      routes.MapRoute(
        "UserProfile",
        "Users/{userId}/{userName}",
        new { controller = "Users", action = "Profile", userName = UrlParameter.Optional },
        new { userId = @"\d+" });

      //routes.MapRoute(
      //  "UpcomingEvents",
      //  "Events/Upcoming",
      //  new {controller = "Events", action = "Index", tab = EventTab.Upcoming});

      //routes.MapRoute(
      //  "ArchiveEvents",
      //  "Events/Archive",
      //  new { controller = "Events", action = "Index", tab = EventTab.Archive });

      routes.MapRoute(
        "EventDetails",
        "Events/{eventId}/{eventSlug}",
        new { controller = "Events", action = "Details", eventSlug = UrlParameter.Optional },
        new { eventId = @"\d+" });

      routes.MapRoute(
          "Default", // Route name
          "{controller}/{action}/{id}", // URL with parameters
          new { controller = "Events", action = "Index", id = UrlParameter.Optional } // Parameter defaults
      );
    }

    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();

      RegisterGlobalFilters(GlobalFilters.Filters);
      RegisterRoutes(RouteTable.Routes);

      DefaultModelBinder.ResourceClassKey = "GlobalResource";

      ModelBinders.Binders.Add(typeof(ICollection<Tag>), new TagsModelBinder());
      ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
      ModelBinders.Binders.Add(typeof (DateTime), new DateTimeModelBinder());
      ModelBinders.Binders.Add(typeof (Event), new EventModelBinder());
      ModelBinders.Binders.Add(typeof (User), new UserModelBinder());

      MvcMiniProfiler.MiniProfilerEF.Initialize();
    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
    }

    protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
    {
      if (!Request.IsAuthenticated)
      {
        return;
      }

      var userId = User.Identity.Name;
      var userRepository = DependencyResolver.Current.GetService<IUserRepository>();

      var user =
        userRepository
          .FindById(int.Parse(userId));

      var identity = new DevAgendaIdentity(user);
      var principal = new GenericPrincipal(identity, null);

      HttpContext.Current.User = principal;
    }

    //protected void Session_End(object sender, EventArgs e)
    //{
    //  var eventsFilter =
    //    Session["EventsFilter"] as EventsFilter;

    //  if (eventsFilter != null && eventsFilter.UserId != 0)
    //  {
    //    try
    //    {
    //      var userRepository =
    //        DependencyResolver.Current.GetService<IUserRepository>();

    //      userRepository
    //        .InsertOrUpdateEventsFilter(eventsFilter);

    //      userRepository.Save();
    //    } 
    //    catch(Exception exc)
    //    {
    //      if (_logger.IsErrorEnabled)
    //      {
    //        _logger
    //          .ErrorException(
    //            string.Format("Error occured while saving EventsFilter for User ({0}).", eventsFilter.UserId),
    //            exc);  
    //      }
    //    }
    //  }
    //}
  }

  // TODO: ME do I have to read tags from db to attach them?
  public class TagsModelBinder : IModelBinder
  {
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      var tagRepository = DependencyResolver.Current.GetService<ITagsRepository>();
      var value = bindingContext.ValueProvider.GetValue("Tags");

      var tags = value.AttemptedValue.Split(' ');
      var tagsCollection = new Collection<Tag>();

      foreach (var tag in tags)
      {
        var existingTag =
          tagRepository.FindByName(tag);

        if (string.IsNullOrEmpty(tag))
        {
          continue;
        }

        tagsCollection.Add(existingTag ?? new Tag { Name = tag });
      }

      return tagsCollection;
    }
  }

  // TODO: LO test test test
  public class DecimalModelBinder : IModelBinder
  {
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
      var modelState = new ModelState { Value = valueResult };

      object actualValue = null;

      try
      {
        //actualValue = Convert.ToDecimal(valueResult.AttemptedValue);
        actualValue = Convert.ToDecimal(valueResult.AttemptedValue, CultureInfo.InvariantCulture);
      }
      catch (FormatException exc)
      {
        // TODO: ME Log sth?

        modelState.Errors.Add(exc);
      }

      bindingContext.ModelState.Add(bindingContext.ModelName, modelState);

      return actualValue;
    }
  }


  // TODO: ME refactor to modelbinderprovider
  public class UserModelBinder : DefaultModelBinder
  {
    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      var userIdProviderResult =
        bindingContext
          .ValueProvider
          .GetValue("userId");

      int userId = 
        int.Parse(
          userIdProviderResult != null 
            ? userIdProviderResult.AttemptedValue 
            : controllerContext.HttpContext.User.Identity.Name);

      var userRepository =
        DependencyResolver
          .Current
          .GetService<IUserRepository>();

      return
        userRepository
          .FindById(userId);
    }
  }

  public class EventModelBinder : DefaultModelBinder
  {
    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      var eventIdProviderResult =
        bindingContext
          .ValueProvider
          .GetValue("eventId");

      if (eventIdProviderResult != null)
      {
        int eventId =
          int.Parse(eventIdProviderResult.AttemptedValue);

        var eventRepository = 
          DependencyResolver
            .Current
            .GetService<IEventRepository>();

        return
          eventRepository
            .FindById(eventId);
      }

      return base.BindModel(controllerContext, bindingContext);
    }
  }

  public class DateTimeModelBinder : IModelBinder
  {
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
      var value =
        bindingContext
          .ValueProvider
          .GetValue(bindingContext.ModelName);

      return
        value
          .ConvertTo(typeof (DateTime), CultureInfo.CurrentCulture);
    }
  }
}