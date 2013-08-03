using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using DevAgenda.Domain;
using DevAgenda.Domain.Events;
using DevAgenda.Domain.Models;
using DevAgenda.Domain.Repositories.Interfaces;
using DevAgenda.Infrastructure;
using DevAgenda.Infrastructure.Geocoding;
using DevAgenda.WebApp.Models;
using Elmah.Contrib.Mvc;

namespace DevAgenda.WebApp.Controllers
{
  [ElmahHandleError]
  public class EventsController : BaseController
  {
    private readonly IEventRepository _eventRepository;
    private readonly IActionRepository _actionRepository;
    private readonly ILocationRepository _locationRepository;

    public EventsController(
      IEventRepository eventRepository, IActionRepository actionRepository, ILocationRepository locationRepository)
    {
      Contract.Requires<ArgumentNullException>(eventRepository != null, "eventRepository");
      Contract.Requires<ArgumentNullException>(actionRepository != null, "actionRepository");
      Contract.Requires<ArgumentNullException>(locationRepository != null, "locationRepository");

      _eventRepository = eventRepository;
      _actionRepository = actionRepository;
      _locationRepository = locationRepository;
    }

    public ActionResult Index(EventsFilter filter)
    {
      var storedFilter =
        CookieSerializer.Get<EventsFilter>("EventsFilter") ?? new EventsFilter();

      if (Request.QueryString.AllKeys.Contains("HappeningTime"))
      {
        storedFilter.HappeningTime = filter.HappeningTime;
      }

      if (filter.IsFreeOfCharge != null || Request.QueryString.AllKeys.Contains("IsFreeOfCharge"))
      {
        storedFilter.IsFreeOfCharge = filter.IsFreeOfCharge;
      }

      if (filter.EventType != null || Request.QueryString.AllKeys.Contains("EventType"))
      {
        storedFilter.EventType = filter.EventType;
      }

      CookieSerializer.Store(storedFilter, "EventsFilter");

      //// 2. Apply QueryString values

      //if (filter.Duration != null || Request.QueryString.AllKeys.Contains("Duration"))
      //{
      //  storedFilter.Duration = filter.Duration;
      //}

      

      //if (filter.LocationId != null || Request.QueryString.AllKeys.Contains("LocationId"))
      //{
      //  storedFilter.LocationId = filter.LocationId;

      //  // TODO: Null and from Session
      //  storedFilter.Location =
      //    storedFilter.LocationId != null
      //      ? _eventRepository.GetSearchedArea(storedFilter.LocationId.Value)
      //      : null;
      //}

      //// 3. Store the filter

      //Session["EventsFilter"] = storedFilter;
      //CookieSerializer.Store(storedFilter, "EventsFilter");

      //// 4. Apply the filter

      var today = DateTime.Now.Date;
      var events = _eventRepository.All;

      switch (storedFilter.HappeningTime)
      {
        case HappeningTime.Upcoming:
          events =
            events
              .Where(e => e.StartDate > today)
              .OrderBy(e => e.StartDate);
          break;
        case HappeningTime.Ongoing:
          events =
            events
              .Where(e => e.StartDate == today || (e.EndDate != null && e.StartDate < today && today <= e.EndDate))
              .OrderBy(e => e.StartDate);
          break;
        default:
          events =
            events
              .Where(e => e.StartDate < today)
              .OrderByDescending(e => e.StartDate);
          break;
      }

      if (storedFilter.IsFreeOfCharge != null)
      {
        events = 
          events
            .Where(e => e.IsFreeOfCharge == storedFilter.IsFreeOfCharge.Value);
      }

      //if (storedFilter.Duration != null)
      //{
      //  //events = events.Where(e => e.EventDurationId == storedFilter.Duration);
      //}

      if (storedFilter.EventType != null)
      {
        events = 
          events
            .Where(e => e.TypeId == storedFilter.EventType.Value);
      }

      //// TODO: HI check for location or locationid?
      //if (storedFilter.Location != null)
      //{
      //  //events =
      //  //  events.Where(
      //  //    e =>
      //  //      e.Latitude >= storedFilter.Location.SouthWestLatitude &&
      //  //      e.Latitude <= storedFilter.Location.NorthEastLatitude &&
      //  //      e.Longitude >= storedFilter.Location.SouthWestLongitude &&
      //  //      e.Longitude <= storedFilter.Location.NorthEastLongitude);
      //}

      var actions =
        _actionRepository
          .All
          .OrderByDescending(a => a.Date);

      return View(
        new MainViewModel
          {
            Events = events,
            Actions = actions,
            Filter = storedFilter
          });
    }

    [Authorize]
    public ActionResult Create()
    {
      PrepareEventTypesData();

      return View();
    }

    [HttpPost]
    [Authorize]
    [Transaction]
    public ActionResult Create(Event @event)
    {
      if (@event.LocationId == 0)
      {
        ModelState
          .AddModelError("LocationPlaceholder", Resources.Event.LocationIsRequired);
      }

      if (@event.EndDate != null && @event.EndDate <= @event.StartDate)
      {
        ModelState
          .AddModelError("EndDate", Resources.Event.EndDateMustBeGreaterThanStartDate);
      }

      if (ModelState.IsValid)
      {
        @event.CreatedById =
          ((DevAgendaIdentity)User.Identity).UserId;

        _eventRepository.InsertOrUpdate(@event);
        _eventRepository.Save();

        DomainEvents.Raise(
          new EventCreated
            {
              Event = @event
            });

        return
          RedirectToAction(
            "Details", 
            "Events", 
            new
              {
                eventId = @event.Id, 
                eventSlug = Utils.Slugify(@event.Name)
              });
      }

      PrepareEventTypesData();

      if (@event.LocationId != 0)
      {
        @event.Location =
          _locationRepository
            .FindById(@event.LocationId);
      }

      return View(@event);
    }

    [HttpPost]
    [Authorize]
    [Transaction]
    [HandleAjaxError]
    public ActionResult RSVP(Event @event, User user)
    {
      Contract.Requires<ArgumentNullException>(@event != null, "event");
      Contract.Requires<ArgumentNullException>(user != null, "user");
      
      @event.Attendees.Add(user);
      
      _eventRepository.InsertOrUpdate(@event);
      _eventRepository.Save();

      DomainEvents.Raise(
        new EventRSVPd
        {
          Event = @event,
          User = user
        });

      return 
        Json(new 
              { 
                UserId = user.Id,
                AvatarLink = user.PictureUrl,
                UserProfileLink = Url.Action("Profile", "Users", new { id = user.Id }),
                UserName = user.UserName
              });
    }

    [HttpPost]
    [Authorize]
    [Transaction]
    [HandleAjaxError]
    public ActionResult UndoRSVP(Event @event, User user)
    {
      Contract.Requires<ArgumentNullException>(@event != null, "event");
      Contract.Requires<ArgumentNullException>(user != null, "user");

      @event.Attendees.Remove(user);

      _eventRepository.InsertOrUpdate(@event);
      _eventRepository.Save();

      DomainEvents.Raise(
        new EventRSVPUndoed
          {
            Event = @event,
            User = user
          });

      return
        Json(new { UserId = user.Id });
    }

    // TODO: HI Error handling
    [HttpPost] //TODO: HI Cache
    public ActionResult GeocodeArea(string area)
    {
      var geocodedAreas =
        Geocoder.GeocodeArea(area);

      var searchedAreas = new List<object>();

      foreach (var geocodedArea in geocodedAreas)
      {
        var searchedArea =
          new SearchedArea
            {
              Id = Guid.NewGuid(),
              NorthEastLatitude = geocodedArea.NorthEastLatitude,
              NorthEastLongitude = geocodedArea.NorthEastLongitude,
              SouthWestLatitude = geocodedArea.SouthWestLatitude,
              SouthWestLongitude = geocodedArea.SouthWestLongitude,
              Name = geocodedArea.Name
            };

        _eventRepository.InsertSearchedArea(searchedArea);

        searchedAreas
          .Add(new
                 {
                   searchedArea.Id,
                   searchedArea.Name
                 });
      }

      _eventRepository.Save();

      return Json(searchedAreas);
    }

    public ActionResult Details(int eventId)
    {
      var @event =
        _eventRepository
          .FindById(eventId);

      return 
        View(@event);
    }

    private void PrepareEventTypesData()
    {
      ViewBag.EventTypes =
        _eventRepository.AllEventTypes
          .ToList()
          .Select(et => new SelectListItem
                          {
                            Text = Resources.Event.ResourceManager.GetString(et.Name),
                            Value = et.Id.ToString()
                          });
    }
  }
}
