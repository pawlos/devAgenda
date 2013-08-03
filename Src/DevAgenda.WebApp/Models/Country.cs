using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevAgenda.WebApp.Models
{
  public class Country
  {
    [Key]
    public string Id { get; set; }

    public string Name { get; set; }
    
    public string Locale { get; set; }

    public virtual ICollection<Event> Events { get; set; }
  }
}