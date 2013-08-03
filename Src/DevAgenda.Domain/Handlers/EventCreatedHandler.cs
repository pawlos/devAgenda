using System;
using System.Diagnostics.Contracts;
using DevAgenda.Domain.Events;
using DevAgenda.Domain.Repositories.Interfaces;

namespace DevAgenda.Domain.Handlers
{
  public class EventCreatedHandler : Handles<EventCreated>
  {
    private readonly IActionRepository _actionRepository;

    public EventCreatedHandler(IActionRepository actionRepository)
    {
      Contract.Requires<ArgumentNullException>(actionRepository != null, "actionRepository");

      _actionRepository = actionRepository;
    }

    // TODO: HI Error handling
    public void Handle(EventCreated args)
    {
      _actionRepository
        .InsertOrUpdate(
          new Models.Action
            {
              Type = Models.ActionType.EventCreated,
              Date = DateTime.UtcNow,
              EventId = args.Event.Id
            }
        );

      _actionRepository.Save();
    }
  }
}
