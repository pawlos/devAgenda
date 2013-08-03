using System;
using System.Diagnostics.Contracts;
using DevAgenda.Domain.Events;
using DevAgenda.Domain.Repositories.Interfaces;

namespace DevAgenda.Domain.Handlers
{
  public class EventRSVPdHandler : Handles<EventRSVPd>
  {
    private readonly IActionRepository _actionRepository;

    public EventRSVPdHandler(IActionRepository actionRepository)
    {
      Contract.Requires<ArgumentNullException>(actionRepository != null, "actionRepository");

      _actionRepository = actionRepository;
    }

    // TODO: HI Error handling
    public void Handle(EventRSVPd args)
    {
      _actionRepository
        .InsertOrUpdate(
          new Models.Action
            {
              Type = Models.ActionType.EventRSVPd,
              Date = DateTime.UtcNow,
              EventId = args.Event.Id,
              UserId = args.User.Id
            });

      _actionRepository.Save();
    }
  }
}
