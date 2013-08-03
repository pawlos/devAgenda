using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using DevAgenda.Domain.Validation;

namespace DevAgenda.Domain.Models
{
  public class Event
  {
    public Event()
    {
      Tags = new Collection<Tag>();
      Attendees = new Collection<User>();
    }

    public int Id { get; set; }

    [Display(Name = "Name", ResourceType = typeof(Resources.Event))]
    [Required(ErrorMessageResourceName = "NameIsRequired", ErrorMessageResourceType = typeof(Resources.Event))]
    [StringLength(50, ErrorMessageResourceName = "NameTooLong", ErrorMessageResourceType = typeof(Resources.Event))]
    public string Name { get; set; }

    [Display(Name = "Type", ResourceType = typeof(Resources.Event))]
    public int TypeId { get; set; }

    [Display(Name = "IsFreeOfCharge", ResourceType = typeof(Resources.Event))]
    public bool IsFreeOfCharge { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "StartDate", ResourceType = typeof(Resources.Event))]
    [Required(ErrorMessageResourceName = "StartDateIsRequired", ErrorMessageResourceType = typeof(Resources.Event))]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "EndDate", ResourceType = typeof(Resources.Event))]
    public DateTime? EndDate { get; set; }

    [UIHint("_EventTags")]
    [Display(Name = "Tags", ResourceType = typeof(Resources.Event))]
    [Tags(ErrorMessageResourceName = "AtLeastOneTagRequired", ErrorMessageResourceType = typeof(Resources.Event))]
    public ICollection<Tag> Tags { get; set; }

    [Display(Name = "Location", ResourceType = typeof(Resources.Event))]
    public int LocationId { get; set; }

    [DataType(DataType.Url)]
    [Display(Name = "Website", ResourceType = typeof(Resources.Event))]
    [Required(ErrorMessageResourceName = "WebsiteIsRequired", ErrorMessageResourceType = typeof(Resources.Event))]
    [RegularExpression(@"((https?):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)", ErrorMessageResourceName = "InvalidUrl", ErrorMessageResourceType = typeof(Resources.Event))]
    public string Website { get; set; }

    [Display(Name = "TwitterHashTag", ResourceType = typeof(Resources.Event))]
    [RegularExpression(@"#\w+", ErrorMessageResourceName = "InvalidTwitterHashTag", ErrorMessageResourceType = typeof(Resources.Event))]
    public string TwitterHashTag { get; set; }

    [Display(Name = "TwitterProfile", ResourceType = typeof(Resources.Event))]
    [RegularExpression(@"@\w+", ErrorMessageResourceName = "InvalidTwitterProfile", ErrorMessageResourceType = typeof(Resources.Event))]
    public string TwitterProfile { get; set; }

    [Display(Name = "Summary", ResourceType = typeof(Resources.Event))]
    [StringLength(500, ErrorMessageResourceName = "SummaryTooLong", ErrorMessageResourceType = typeof(Resources.Event))]
    public string Summary { get; set; }

    public ICollection<User> Attendees { get; set; }

    // TODO: LO CreatedById is required but I want to populate it after POST, any way to change this?
    public int? CreatedById { get; set; }

    [ForeignKey("TypeId")]
    public EventType Type { get; set; }

    [ForeignKey("CreatedById")]
    public User CreatedBy { get; set; }

    [ForeignKey("LocationId")]
    public Location Location { get; set; }
  }
}