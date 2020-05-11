using EventManager.Data.Core.Repositories;
using EventManager.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Persistence.Repositories
{
    public class EventTypeRepository : Repository<EventType>, IEventTypeRepository
    {
        public EventManagerContext EventManagerContext {
            get { return context as EventManagerContext; }
        }
        public EventTypeRepository(EventManagerContext context) 
            : base(context)
        {
        }

        public async Task<IEnumerable<EventType>> GetEventTypesWithEventsAsync(int filterByEventTypeId = 0)
        {
            return await EventManagerContext.EventTypes
                .Where(x => (filterByEventTypeId == 0 || x.Id == filterByEventTypeId))
                .Include(x => x.Events)
                .ToListAsync();
        }
    }
}
