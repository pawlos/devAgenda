using DevAgenda.Domain.Models;

namespace DevAgenda.WebApp.Models
{
  public class EventDetailsViewModel
  {
    public Event Event { get; set; }
    public bool DisplayCountdown { get; set; }
    public bool DisplayExtended { get; set; }
  }
}