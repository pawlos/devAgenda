using System.Collections.Generic;
using DevAgenda.Domain.Models;

namespace DevAgenda.WebApp.Models
{
  public class MainViewModel
  {
    public IEnumerable<Event> Events { get; set; }
    public IEnumerable<Action> Actions { get; set; }
    public EventTab ActiveTab { get; set; }

    public EventsFilter Filter { get; set; }
  }
}