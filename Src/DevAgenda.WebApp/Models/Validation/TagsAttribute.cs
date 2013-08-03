using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace DevAgenda.WebApp.Models.Validation
{
  public class TagsAttribute : ValidationAttribute
  {
    public override bool IsValid(object value)
    {
      var collection = value as ICollection;

      return collection != null && collection.Count > 0;
    }
  }
}