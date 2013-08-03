using System.ComponentModel.DataAnnotations;

namespace DevAgenda.WebApp.Models.Validation
{
  public class LocationAttribute : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      return base.IsValid(value, validationContext);
    }

    public override bool IsValid(object value)
    {
      var location = value as Location;

      return
        location != null && (location.City != null && location.Country != null);
    }
  }
}