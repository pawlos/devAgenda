using System;
using System.ComponentModel.DataAnnotations;

namespace DevAgenda.Domain.Models
{
  public class Action
  {
    public int Id { get; set; }

    public ActionType Type
    {
      get
      {
        return (ActionType)TypeValue;
      }
      set
      {
        TypeValue = (int)value;
      }
    }

    public DateTime Date { get; set; }

    public int TypeValue { get; set; }

    [ForeignKey("EventId")]
    public Event Event { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    public int? EventId { get; set; }

    public int? UserId { get; set; }
  }
}
