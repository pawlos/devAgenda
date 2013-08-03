using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace DevAgenda.Infrastructure
{
  public static class CookieSerializer
  {
    public static T Get<T>(string cookieName) where T : class
    {
      var cookie = 
        HttpContext.Current
          .Request
          .Cookies[cookieName];

      T storedValue = null;

      if (cookie != null)
      {
        var buffer =
          Convert.FromBase64String(cookie.Value);

        using (var stream = new MemoryStream(buffer))
        {
          var formatter = new BinaryFormatter();

          storedValue =
            formatter.Deserialize(stream) as T;
        }
      }

      return 
        storedValue;
    }

    public static void Store(Object @object, string cookieName)
    {
      var cookie =
        new HttpCookie(cookieName)
        {
          Expires = DateTime.Now.AddYears(15)
        };

      using (var stream = new MemoryStream())
      {
        var formatter = new BinaryFormatter();
        formatter.Serialize(stream, @object);

        stream.Seek(0, SeekOrigin.Begin);

        var buffer = 
          new byte[stream.Length];

        stream.Read(buffer, 0, (int)stream.Length);

        cookie.Value = 
          Convert.ToBase64String(buffer);

        HttpContext.Current.Response.Cookies.Add(cookie);
      }
    }
  }
}
