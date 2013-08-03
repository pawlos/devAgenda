using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using DevAgenda.Domain.Models;
using Newtonsoft.Json.Linq;

namespace DevAgenda.Infrastructure.Geocoding
{
  public static class Geocoder
  {
    public class GeocodedArea
    {
      public decimal NorthEastLatitude { get; set; }
      public decimal NorthEastLongitude { get; set; }
      public decimal SouthWestLatitude { get; set; }
      public decimal SouthWestLongitude { get; set; }

      public string Name { get; set; }
    }

    // NOTE: http://code.google.com/apis/maps/faq.html#languagesupport
    private static readonly StringDictionary _locales =
      new StringDictionary
        {
          {"bg", "bg"},
          {"cn", "zh-CN"},
          {"cz", "cs"},
          {"dk", "da"},
          {"de", "de"},
          {"gb", "en"},
          {"gr", "el"},
          {"es", "es"},
          {"fi", "fi"},
          {"ph", "fil"},
          {"fr", "fr"},
          {"hr", "hr"},
          {"hu", "hu"},
          {"id", "id"},
          {"it", "it"},
          {"il", "iw"},
          {"jp", "ja"},
          {"kr", "ko"},
          {"lt", "lt"},
          {"lv", "lv"},
          {"nl", "nl"},
          {"no", "no"},
          {"pl", "pl"},
          {"pt", "pt"},
          {"ro", "ro"},
          {"ru", "ru"},
          {"sk", "sk"},
          {"si", "sl"},
          {"se", "sv"},
          {"th", "th"},
          {"tr", "tr"},
          {"ua", "uk"},
          {"us", "en"},
          {"vn", "vi"},
        };

    // TODO: ME error handling
    private static JObject GetJSON(string requestUrl)
    {
      JObject parsedResponse = null;

      var request = WebRequest.Create(requestUrl);

      using (var responseStream = request.GetResponse().GetResponseStream())
      {
        if (responseStream != null)
        {
          using (var reader = new StreamReader(responseStream))
          {
            string response = reader.ReadToEnd();

            if (!string.IsNullOrEmpty(response))
            {
              parsedResponse = JObject.Parse(response);
            }
          }
        }
      }

      return parsedResponse;
    }

    private static bool IsInPoliticalType(JToken token, string type)
    {
      var types = token["types"].Children();

      return
        types.Contains(type) && types.Contains("political");
    }

    private static bool IsCountry(JToken token)
    {
      return IsInPoliticalType(token, "country");
    }

    private static bool IsLocality(JToken token)
    {
      return IsInPoliticalType(token, "locality");
    }

    private static bool IsAdminAreaLevel1(JToken token)
    {
      return IsInPoliticalType(token, "administrative_area_level_1");
    }

    private static bool HasBounds(JToken token)
    {
      return
        token["geometry"]
          .Children()
          .Any(t =>
                 {
                   var jProperty = t as JProperty;
                   return jProperty != null && jProperty.Name == "bounds";
                 });
    }

    public static IEnumerable<GeocodedLocation> Geocode(string locationsQuery)
    {
      var geocodeUrl =
        string.Format(@"http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=false&language={1}", locationsQuery, "en");

      JObject parsedResponse = GetJSON(geocodeUrl);

      var geocodedLocations =
        from r in parsedResponse["results"]
        let country = (from ac in r["address_components"]
                       where IsCountry(ac)
                       select ac["short_name"]).SingleOrDefault()
        where IsLocality(r) && country != null
        select
          new GeocodedLocation
            {
              Latitude = decimal.Parse(r["geometry"]["location"]["lat"].ToString()),
              Longitude = decimal.Parse(r["geometry"]["location"]["lng"].ToString()),
              Locale = _locales.ContainsKey(country.ToString()) ? _locales[country.ToString()] : "en"
            };

      return Enumerable.ToList<GeocodedLocation>(geocodedLocations);
    }

    public static Location ReverseGeocode(GeocodedLocation location)
    {
      var reverseGeocodeUrl =
        string.Format(
          @"http://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&sensor=false&language={2}",
          location.Latitude.ToString(CultureInfo.InvariantCulture),
          location.Longitude.ToString(CultureInfo.InvariantCulture),
          location.Locale);

      JObject parsedResponse = GetJSON(reverseGeocodeUrl);

      var reverseGeocodedLocation =
        from r in parsedResponse["results"]
        let city = r["address_components"].SingleOrDefault(IsLocality)
        let adminAreaLevel1 = r["address_components"].SingleOrDefault(IsAdminAreaLevel1)
        let country = r["address_components"].SingleOrDefault(IsCountry)
        where IsLocality(r)
        select new Location
                 {
                   City = city["long_name"].ToString(),
                   AdministrativeArea = adminAreaLevel1 == null ? null : adminAreaLevel1["short_name"].ToString(),
                   Country = country["long_name"].ToString(),
                   CountryCode = country["short_name"].ToString(),
                   Latitude = location.Latitude,
                   Longitude = location.Longitude
                 };

      // TODO: ME log if more than one result single() fail
      return reverseGeocodedLocation.FirstOrDefault();
    }

    // TODO: ME think of locales
    public static IEnumerable<GeocodedArea> GeocodeArea(string area)
    {
      // TODO: HI locale set to current UI's
      var geocodeUrl =
        string.Format(@"http://maps.googleapis.com/maps/api/geocode/json?address={0}&sensor=false&language={1}", area, "en");

      JObject parsedResponse = GetJSON(geocodeUrl);

      var geocodedAreas =
        from r in parsedResponse["results"]
        let country = (from ac in r["address_components"]
                       where IsCountry(ac)
                       select ac["short_name"]).SingleOrDefault()
        where HasBounds(r) && country != null
        select
          new GeocodedArea
            {
              NorthEastLatitude = decimal.Parse(r["geometry"]["bounds"]["northeast"]["lat"].ToString()),
              NorthEastLongitude = decimal.Parse(r["geometry"]["bounds"]["northeast"]["lng"].ToString()),
              SouthWestLatitude = decimal.Parse(r["geometry"]["bounds"]["southwest"]["lat"].ToString()),
              SouthWestLongitude = decimal.Parse(r["geometry"]["bounds"]["southwest"]["lng"].ToString()),
              Name = r["formatted_address"].ToString()
            };

      return geocodedAreas.ToList();
    }
  }
}