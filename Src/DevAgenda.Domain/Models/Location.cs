using System.ComponentModel.DataAnnotations;

namespace DevAgenda.Domain.Models
{
  public class Location
  {
    public int Id { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    [Required]
    public string City { get; set; }

    public string AdministrativeArea { get; set; }

    [Required]
    public string Country { get; set; }

    public string CountryCode { get; set; }

    public string Formatted
    {
      get
      {
        if (City == null)
        {
          return null;
        }

        return
          City +
          (!string.IsNullOrEmpty(AdministrativeArea)
             ? ", " + AdministrativeArea
             : "") +
          ", " + Country;
      }
    }
  }
}