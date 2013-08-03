using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace DevAgenda.Domain.Validation
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