using System;
using System.Text;
using System.Text.RegularExpressions;

namespace DevAgenda.Infrastructure
{
  public static class Utils
  {
    public static string Slugify(string text)
    {
      var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(text);
      var slugified = Encoding.ASCII.GetString(bytes);

      slugified = Regex.Replace(slugified, @"[^a-zA-Z0-9\s-]", "");
      slugified = Regex.Replace(slugified, @"\s+", " ").Trim();
      slugified = slugified.Substring(0, slugified.Length <= 45 ? slugified.Length : 45).Trim();
      slugified = Regex.Replace(slugified, @"\s", "-");

      return 
        slugified.ToLower();
    }

    public static string FormatDateRange(DateTime startDate, DateTime? endDate)
    {
      if (endDate == null)
      {
        return
          startDate.Day + " " +
          startDate.ToString("MMM") + " " +
          startDate.Year;
      }

      if (startDate.Year == endDate.Value.Year)
      {
        if (startDate.Month == endDate.Value.Month)
        {
          return
            startDate.ToString("dd") + " - " + endDate.Value.ToString("dd") + " " +
            startDate.ToString("MMM") + " " +
            startDate.Year;
        }

        return
          startDate.ToString("dd MMM") + " - " +
          endDate.Value.ToString("dd MMM") + " " +
          startDate.Year;
      }

      return
        startDate.ToString("dd MMM yyyy") + " - " +
        endDate.Value.ToString("dd MMM yyyy");
    }
  }
}