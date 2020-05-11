using EventManager.Data.Core.Repositories;
using EventManager.Models.Domain;
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
        public async Task<IEnumerable<Event>> GetEventsWithCreatorEventTypeAndSeriesAsnyc(int filterByEventTypeId = 0, int filterByCreatorUserId = 0, int filterByEventSeriesId = 0)
        {
            return await EventManagerContext.Events
                .Where(x => (filterByEventTypeId == 0 || x.EventTypeId == filterByEventTypeId) && (filterByCreatorUserId == 0 || x.CreatorId == filterByCreatorUserId) && (filterByEventSeriesId == 0 || x.EventSeriesId == filterByEventSeriesId) )
                    .Include(x => x.Creator)
                        .ThenInclude(x => x.Rank)
                    .Include(x => x.EventType)
                    .Include(x => x.EventSeries)
                    .ToListAsync();
        }

    }
}
