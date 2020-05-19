using EventManager.Data.Core.Repositories;
using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Persistence.Repositories
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventManagerContext EventManagerContext {
            get { return context as EventManagerContext; }
        }
        public EventRepository(EventManagerContext context) 
            : base(context)
        {
        }
        /// <summary>
        /// Async method that returns a list of events with eager-loaded Creator(User) and EventType(EventType)
        /// </summary>
        /// <param name="filterByEventTypeId">Optional parameter to filter the result by only events with the given EventTypeId</param>
        /// <param name="filterByCreatorUserId">Optional parameter to filter the result by only events with the given Creator UserId</param>
        /// /// <param name="filterByEventSeriesId">Optional parameter to filter the result by only events with the given Event Series Id</param>
        /// <returns>a Task<List<Event>> of the matching Events.</Event></returns>
        public async Task<IEnumerable<Event>> GetEventsWithCreatorEventTypeAndSeriesAsnyc(string searchString = "", int filterByEventTypeId = 0, int filterByCreatorUserId = 0, int filterByEventSeriesId = 0, int page = 1, int pageSize = 25)
        {
            return await EventManagerContext.Events
                .Where(x => (string.IsNullOrEmpty(searchString) || x.Title.Contains(searchString)) && (filterByEventTypeId == 0 || x.EventTypeId == filterByEventTypeId) && (filterByCreatorUserId == 0 || x.OwnerId == filterByCreatorUserId) && (filterByEventSeriesId == 0 || x.EventSeriesId == filterByEventSeriesId) )
                .Skip((page-1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Owner)
                        .ThenInclude(x => x.Rank)
                .Include(x => x.EventType)
                .Include(x => x.EventSeries)
                .Include(x => x.Registrations)
                    .ToListAsync();
        }
        public IEnumerable<Event> GetEventsWithCreatorEventTypeAndSeries(string searchString = "", int filterByEventTypeId = 0, int filterByCreatorUserId = 0, int filterByEventSeriesId = 0, int page = 1, int pageSize = 25)
        {
            return EventManagerContext.Events
                .Where(x => (string.IsNullOrEmpty(searchString) || x.Title.Contains(searchString)) && (filterByEventTypeId == 0 || x.EventTypeId == filterByEventTypeId) && (filterByCreatorUserId == 0 || x.OwnerId == filterByCreatorUserId) && (filterByEventSeriesId == 0 || x.EventSeriesId == filterByEventSeriesId))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Owner)
                        .ThenInclude(x => x.Rank)
                    .Include(x => x.EventType)
                    .Include(x => x.EventSeries)
                    .ToList();
        }
        public async Task<Event> GetEventWithCreatorEventTypeAndSeriesAsync(int id)
        {
            return await EventManagerContext.Events
                .Where(x => x.Id == id)
                .Include(x => x.Owner)
                    .ThenInclude(x => x.Rank)
                .Include(x => x.EventType)
                .Include(x => x.EventSeries)
                .FirstOrDefaultAsync();
        }
        public Event GetEventWithCreatorEventTypeAndSeries(int id)
        {
            return EventManagerContext.Events
                .Where(x => x.Id == id)
                .Include(x => x.Owner)
                    .ThenInclude(x => x.Rank)
                .Include(x => x.EventType)
                .Include(x => x.EventSeries)
                .FirstOrDefault();
        }
        public async Task<Event> GetEventWithCreatorEventTypeSeriesAndRegistrationsAsync(int id)
        {
            return await EventManagerContext.Events
                .Where(x => x.Id == id)
                .Include(x => x.Owner)
                    .ThenInclude(x => x.Rank)
                .Include(x => x.EventType)
                .Include(x => x.EventSeries)
                .Include(x => x.Registrations)
                .FirstOrDefaultAsync();
        }
        public Event GetEventWithCreatorEventTypeSeriesAndRegistrations(int id)
        {
            return EventManagerContext.Events
                .Where(x => x.Id == id)
                .Include(x => x.Owner)
                    .ThenInclude(x => x.Rank)
                .Include(x => x.EventType)
                .Include(x => x.EventSeries)
                .Include(x => x.Registrations)
                .FirstOrDefault();
        }
        public async Task<IEnumerable<Event>> GetEventsWithRegistrationsForEventTypeIdAsync(int eventTypeId)
        {
            return await EventManagerContext.Events
                .Where(x => x.EventTypeId == eventTypeId)
                .Include(x => x.Owner)
                    .ThenInclude(x => x.Rank)
                .Include(x => x.Registrations)
                .ToListAsync();
        }
        public IEnumerable<Event> GetEventsWithRegistrationsForEventTypeId(int eventTypeId)
        {
            return EventManagerContext.Events
                .Where(x => x.EventTypeId == eventTypeId)
                .Include(x => x.Owner)
                    .ThenInclude(x => x.Rank)
                .Include(x => x.Registrations)
                .ToList();
        }
        public async Task<IEnumerable<Event>> GetEventsWithRegistrationsForEventSeriesIdAsync(int eventSeriesId)
        {
            return await EventManagerContext.Events
                .Where(x => x.EventSeriesId == eventSeriesId)
                .Include(x => x.Owner)
                    .ThenInclude(x => x.Rank)
                .Include(x => x.Registrations)
                .ToListAsync();
        }
        public IEnumerable<Event> GetEventsWithRegistrationsForEventSeriesId(int eventSeriesId)
        {
            return EventManagerContext.Events
                .Where(x => x.EventSeriesId == eventSeriesId)
                .Include(x => x.Owner)
                    .ThenInclude(x => x.Rank)
                .Include(x => x.Registrations)
                .ToList();
        }
        public SelectList GetEventSelectList()
        {
            return new SelectList(EventManagerContext.Events, nameof(Event.Id), nameof(Event.Title));
        }
        public async Task<SelectList> GetEventSelectListAsync()
        {
            return new SelectList(await EventManagerContext.Events.ToListAsync(), nameof(Event.Id), nameof(Event.Title));
        }
        public async Task<IEnumerable<Event>> GetEventsOwnedByUserByUserIdAsync(int userId)
        {
            return await EventManagerContext.Events
                .Where(x => x.OwnerId == userId)
                .Include(x => x.EventType)
                .Include(x => x.EventSeries)
                .Include(x => x.Registrations)
                .Include(x => x.Owner)
                    .ThenInclude(x => x.Rank)
                .ToListAsync();
        }
        public IEnumerable<Event> GetEventsOwnedByUserByUserId(int userId)
        {
            return EventManagerContext.Events
                .Where(x => x.OwnerId == userId)
                .Include(x => x.EventType)
                .Include(x => x.EventSeries)
                .Include(x => x.Registrations)
                .Include(x => x.Owner)
                    .ThenInclude(x => x.Rank)
                .ToList();
        }
        public Event GetEventWithRegistrations(int id)
        {
            return EventManagerContext.Events
                .Include(x => x.Registrations)
                .FirstOrDefault(x => x.Id == id);
        }
        public async Task<Event> GetEventWithRegistrationsAsync(int id)
        {
            return await EventManagerContext.Events
                .Include(x => x.Registrations)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
