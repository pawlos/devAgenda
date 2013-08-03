using DevAgenda.Domain.Models;

namespace DevAgenda.Domain.Events
{
  public class EventRSVPUndoed : IDomainEvent
  {
    public Event Event { get; set; }
    public User User { get; set; }
  }
}
