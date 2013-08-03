using System.Collections.Generic;
using NLog;

namespace DevAgenda.Domain
{
  public static class DomainEvents
  {
    private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public static IContainer Container { get; set; }

    public static void Raise<T>(T args) where T : IDomainEvent
    {
      if (Container != null)
      {
        foreach (var handler in Container.ResolveAll<Handles<T>>())
        {
          handler.Handle(args);
        }
      }
      else
      {
        _logger.Warn(() => "Domain Events Container is not defined. (NULL)");
      }
    }
  }

  public interface IDomainEvent { }

  public interface Handles<T> where T : IDomainEvent
  {
    void Handle(T args);
  }

  public interface IContainer
  {
    IEnumerable<T> ResolveAll<T>();
  }
}
