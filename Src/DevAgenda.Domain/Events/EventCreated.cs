using DevAgenda.Domain.Models;

namespace DevAgenda.Domain.Events
{
  public class EventCreated : IDomainEvent
  {
    public Event Event { get; set; }
  }
}
