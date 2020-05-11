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

        public async Task<IEnumerable<EventSeries>> GetEventSeriesWithEventsAsync(int filterByEventSeriesId = 0)
        {
            return await EventManagerContext.EventSeries
                .Where(x => (filterByEventSeriesId == 0 || x.Id == filterByEventSeriesId))
                .Include(x => x.Events)
                    .ThenInclude(x => x.EventType)
                .ToListAsync();
        }

        
    }
}
