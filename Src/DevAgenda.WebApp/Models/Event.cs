using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using DevAgenda.WebApp.Models.Validation;

namespace DevAgenda.WebApp.Models
{
  public class Event
  {
    public Event()
    {
      Tags = new Collection<Tag>();
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
    [Display(Name = "Date", ResourceType = typeof(Resources.Event))]
    [Required(ErrorMessageResourceName = "DateIsRequired", ErrorMessageResourceType = typeof(Resources.Event))]
    public DateTime Date { get; set; }

    [Display(Name="Lasts for")]
    public int EventDurationId { get; set; }

    [UIHint("_EventTags")]
    [Tags(ErrorMessageResourceName = "AtLeastOneTagRequired", ErrorMessageResourceType = typeof(Resources.Event))]
    public ICollection<Tag> Tags { get; set; }

    //[UIHint("_Location")]
    //[Location(ErrorMessageResourceName = "CityIsRequired", ErrorMessageResourceType = typeof(Resources.Event))]
    //[Display(Name="City")]
    //public Location Location { get; set; }

    [Display(Name = "Details", ResourceType = typeof(Resources.Event))]
    [StringLength(1000, ErrorMessageResourceName = "DescriptionTooLong", ErrorMessageResourceType = typeof(Resources.Event))]
    public string Details { get; set; }

    [DataType(DataType.Url)]
    [Display(Name = "Website")]
    [RegularExpression(@"((https?):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)", ErrorMessageResourceName = "InvalidUrl", ErrorMessageResourceType = typeof(Resources.Event))]
    public string Website { get; set; }

    [Display(Name = "TwitterHashTag", ResourceType = typeof(Resources.Event))]
    [RegularExpression(@"#\w+", ErrorMessageResourceName = "InvalidTwitterHashTag", ErrorMessageResourceType = typeof(Resources.Event))]
    public string TwitterHashTag { get; set; }
    
    // TODO: LO CreatedById is required but I want to populate it after POST, any way to change this?
    public int? CreatedById { get; set; }

    [ForeignKey("TypeId")]
    public EventType Type { get; set; }

    [ForeignKey("EventDurationId")]
    public EventDuration EventDuration { get; set; }

    [ForeignKey("CreatedById")]
    public User CreatedBy { get; set; }


    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    [Required]
    public string City { get; set; }

    public string AdministrativeArea { get; set; }

    [Required]
    public string Country { get; set; }

    public string FormattedLocation
    {
      get
      {
        if (City == null)
        {
          return null;
        }

        return
          City +
          (!string.IsNullOrEmpty(AdministrativeArea) ? ", " + AdministrativeArea : "") +
          ", " + Country; 
      }
    }

  }

  public class Location
  {
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    [Required]
    public string City { get; set; }

    public string AdministrativeArea { get; set; }

    [Required]
    public string Country { get; set; }

    public string Formatted()
    {
      if (City == null)
      {
        return null;
      }

      return
        City +
        (!string.IsNullOrEmpty(AdministrativeArea)
          ? ", " + AdministrativeArea
          : "") +
        ", " + Country;
    }
  }
}