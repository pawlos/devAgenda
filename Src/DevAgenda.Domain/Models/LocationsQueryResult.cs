using System.ComponentModel.DataAnnotations;

namespace DevAgenda.Domain.Models
{
  public class LocationsQueryResult
  {
    public int Id { get; set; }

    [MaxLength(50)]
    public string Query { get; set; }
    public int? LocationId { get; set; }

    [ForeignKey("LocationId")]
    public Location Location { get; set; }
  } 
}
