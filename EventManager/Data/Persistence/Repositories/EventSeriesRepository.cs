using EventManager.Data.Core.Repositories;
using EventManager.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Persistence.Repositories
{
    public class EventSeriesRepository : Repository<EventSeries>, IEventSeriesRepository
    {
        public EventManagerContext EventManagerContext {
            get { return context as EventManagerContext; }
        }
        public EventSeriesRepository(EventManagerContext context)
            :base(context)
        {
        }

        public async Task<IEnumerable<EventSeries>> GetEventSeriesWithEventsAsync(int filterByEventSeriesId = 0, int page = 1, int pageSize = 25)
        {
            return await EventManagerContext.EventSerieses
                .Where(x => (filterByEventSeriesId == 0 || x.Id == filterByEventSeriesId))
                .Skip((page -1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Events)
                    .ThenInclude(x => x.EventType)
                .ToListAsync();
        }

        
    }
}
