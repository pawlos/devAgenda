using System;
using System.ComponentModel.DataAnnotations;

namespace DevAgenda.Domain.Models
{
  [Serializable]
  public class EventsFilter
  {
    [NonSerialized]
    private User _user;

    public EventsFilter()
      : this(0)
    { }

    public EventsFilter(int userId)
    {
      UserId = userId;
    }

    public int Id { get; set; }

    public bool? IsFreeOfCharge { get; set; }
    public int? EventType { get; set; }

    [ForeignKey("UserId")]
    public User User
    {
      get { return _user; }
      set { _user = value; }
    }

    public int UserId { get; set; }

    public Guid? LocationId { get; set; }

    [ForeignKey("LocationId")]
    public SearchedArea Location { get; set; }

    public int? Duration { get; set; }

    public HappeningTime HappeningTime { get; set; }
  }

  public enum HappeningTime 
  {
    Upcoming = 0,
    Ongoing = 1,
    Past = 2
  }
}