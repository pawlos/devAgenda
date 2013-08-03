using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevAgenda.Domain.Models
{
  public class User
  {
    public int Id { get; set; }

    [Required]
    public string ClaimedId { get; set; }

    [Required]
    public string UserName { get; set; }

    // TODO: HI let it be int and wait till EF will support Enums
    [Required]
    public int AuthenticatedWith { get; set; }

    public string PictureUrl { get; set; }

    public virtual ICollection<Event> Events { get; set; }
  }
}