using System;
using DevAgenda.Resources;

namespace DevAgenda.WebApp.Helpers
{
  public static class DateTimeExtensions
  {
    public static string DaysFromNow(this DateTime toDate)
    {
      int daysFromNow = 
        toDate.Subtract(DateTime.Now.Date).Days;

      if (daysFromNow == 0)
      {
        return Common.Today;
      }

      return 
        daysFromNow == 1 
          ? Common.Tomorrow 
          : string.Format(Common.InDays, daysFromNow);
    }
  }
}