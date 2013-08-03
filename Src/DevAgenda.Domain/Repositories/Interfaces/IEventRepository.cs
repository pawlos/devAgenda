using System;
using System.Collections.Generic;
using DevAgenda.Domain.Models;

namespace DevAgenda.Domain.Repositories.Interfaces
{
  public interface IEventRepository : IRepository<Event>
  {
    IEnumerable<EventType> AllEventTypes { get; }
    void InsertSearchedArea(SearchedArea searchedArea);
    SearchedArea GetSearchedArea(Guid searchedAreaId);
    void AddRSVP(int eventId, int userId);
  }
}
