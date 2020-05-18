using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<IEnumerable<Event>> GetEventsWithCreatorEventTypeAndSeriesAsnyc(string searchString = "", int filterByEventTypeId = 0, int filterByCreatorUserId = 0, int filterByEventSeriesId = 0, int page = 1, int pageSize = 25);
        IEnumerable<Event> GetEventsWithCreatorEventTypeAndSeries(string searchString = "", int filterByEventTypeId = 0, int filterByCreatorUserId = 0, int filterByEventSeriesId = 0, int page = 1, int pageSize = 25);
        Task<Event> GetEventWithCreatorEventTypeAndSeriesAsync(int id);
        Event GetEventWithCreatorEventTypeAndSeries(int id);
        Task<Event> GetEventWithCreatorEventTypeSeriesAndRegistrationsAsync(int id);
        Event GetEventWithCreatorEventTypeSeriesAndRegistrations(int id);
        Task<IEnumerable<Event>> GetEventsWithRegistrationsForEventTypeIdAsync(int eventTypeId);
        IEnumerable<Event> GetEventsWithRegistrationsForEventTypeId(int eventTypeId);
        Task<IEnumerable<Event>> GetEventsWithRegistrationsForEventSeriesIdAsync(int eventSeriesId);
        IEnumerable<Event> GetEventsWithRegistrationsForEventSeriesId(int eventSeriesId);
        Task<IEnumerable<Event>> GetEventsOwnedByUserByUserIdAsync(int userId);
        IEnumerable<Event> GetEventsOwnedByUserByUserId(int userId);
        SelectList GetEventSelectList();
        Task<SelectList> GetEventSelectListAsync();
        Event GetEventWithRegistrations(int id);
        Task<Event> GetEventWithRegistrationsAsync(int id);
    }
}
