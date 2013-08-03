using System;
using System.ComponentModel.DataAnnotations;

namespace DevAgenda.Domain.Models
{
  [Serializable]
  public class SearchedArea
  {
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }

    public decimal NorthEastLatitude { get; set; }
    public decimal NorthEastLongitude { get; set; }
    public decimal SouthWestLatitude { get; set; }
    public decimal SouthWestLongitude { get; set; }
    public string Name { get; set; }
  }
}
