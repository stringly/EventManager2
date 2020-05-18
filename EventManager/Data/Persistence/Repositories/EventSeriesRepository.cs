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
    public class EventSeriesRepository : Repository<EventSeries>, IEventSeriesRepository
    {
        public EventManagerContext EventManagerContext {
            get { return context as EventManagerContext; }
        }
        public EventSeriesRepository(EventManagerContext context)
            :base(context)
        {
        }

        public async Task<IEnumerable<EventSeries>> GetEventSeriesWithEventsAsync(string searchString = "", int page = 1, int pageSize = 25)
        {
            return await EventManagerContext.EventSerieses
                .Where(x => (string.IsNullOrEmpty(searchString) || x.Title.ToLower().Contains(searchString)))
                .Skip((page -1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Events)
                    .ThenInclude(x => x.EventType)
                .ToListAsync();
        }
        public async Task<EventSeries> GetEventSeriesWithEventsAndRegistrationsAsync(int id)
        {
            return await EventManagerContext.EventSerieses
                .Include(x => x.Events)
                    .ThenInclude(x => x.Registrations)
                .Include(x => x.Events)
                    .ThenInclude(x => x.EventSeries)
                .Include(x => x.Events)
                    .ThenInclude(x => x.EventType)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public EventSeries GetEventSeriesWithEventsAndRegistrations(int id)
        {
            return EventManagerContext.EventSerieses
                .Include(x => x.Events)
                    .ThenInclude(x => x.Registrations)
                .Include(x => x.Events)
                    .ThenInclude(x => x.EventSeries)
                .Include(x => x.Events)
                    .ThenInclude(x => x.EventType)
                .FirstOrDefault(x => x.Id == id);
        }
        public SelectList GetEventSeriesSelectList()
        {
            return new SelectList(EventManagerContext.EventSerieses, nameof(EventSeries.Id), nameof(EventSeries.Title));
        }
        public async Task<SelectList> GetEventSeriesSelectListAsync()
        {
            return new SelectList(await EventManagerContext.EventSerieses.ToListAsync(), nameof(EventSeries.Id), nameof(EventSeries.Title));
        }

    }
}
