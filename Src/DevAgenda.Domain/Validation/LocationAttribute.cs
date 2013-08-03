using System.ComponentModel.DataAnnotations;
using DevAgenda.Domain.Models;

namespace DevAgenda.Domain.Validation
{
  public class LocationAttribute : ValidationAttribute
  {
    public override bool IsValid(object value)
    {
      var location = value as Location;

      return
        location != null && (location.City != null && location.Country != null);
    }
  }
}