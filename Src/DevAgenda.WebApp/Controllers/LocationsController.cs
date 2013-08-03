using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using DevAgenda.Domain.Models;
using DevAgenda.Domain.Repositories.Interfaces;
using DevAgenda.Infrastructure;
using DevAgenda.Infrastructure.Geocoding;
using DevAgenda.WebApp.Services;
using Elmah.Contrib.Mvc;
using NLog;

namespace DevAgenda.WebApp.Controllers
{
  [ElmahHandleError]
  public class LocationsController : Controller
  {
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    private readonly ILocationsQueryCache _locationsQueryCache;
    private readonly ILocationRepository _locationRepository;

    public LocationsController(ILocationsQueryCache locationsQueryCache, ILocationRepository locationRepository)
    {
      Contract.Requires<ArgumentNullException>(locationsQueryCache != null, "locationsQueryCache");
      Contract.Requires<ArgumentNullException>(locationRepository != null, "loactionRepository");

      _locationsQueryCache = locationsQueryCache;
      _locationRepository = locationRepository;
    }

    [HttpPost]
    [Authorize]
    [Transaction]
    [OutputCache(Duration = 3600, VaryByParam = "locationsQuery")]
    [HandleAjaxError(ErrorMessageResourceName = "ErrorWhileGeocoding", ErrorMessageResourceType = typeof(Resources.Common))]
    public ActionResult Geocode(string locationsQuery)
    {
      var locations =
        _locationsQueryCache
          .Get(locationsQuery);
      
      if (locations == null)
      {
        var geocodedLocations =
          Geocoder.Geocode(locationsQuery).ToList();

        var locationsToReturn =
          new List<Location>();

        foreach (var geocodedLocation in geocodedLocations)
        {
          var location =
            _locationRepository
              .FindByCoords(
                geocodedLocation.Latitude,
                geocodedLocation.Longitude);

          if (location == null)
          {
            var revGeocodedLocation =
              Geocoder.ReverseGeocode(geocodedLocation);

            if (revGeocodedLocation == null)
            {
              if (_logger.IsWarnEnabled)
              {
                _logger
                  .Warn(() =>
                        string.Format(
                          "Could not reverse geocode coordinates: Lat({0}), Lng({1}).",
                          geocodedLocation.Latitude,
                          geocodedLocation.Longitude));
              }

              continue;
            }

            _locationRepository
              .InsertOrUpdate(revGeocodedLocation);

            _locationRepository.Save();

            locationsToReturn
              .Add(revGeocodedLocation);
          }
          else
          {
            locationsToReturn
              .Add(location);
          }
        }

        locations = locationsToReturn;
        
        _locationsQueryCache
          .Set(locationsQuery, locations);
      }

      return
        Json(
          locations
            .Select(l => new { l.Id, l.Formatted }));
    }
  }
}
