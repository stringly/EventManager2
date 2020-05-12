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
    public class EventTypeRepository : Repository<EventType>, IEventTypeRepository
    {
        public EventManagerContext EventManagerContext {
            get { return context as EventManagerContext; }
        }
        public EventTypeRepository(EventManagerContext context) 
            : base(context)
        {
        }

        public async Task<IEnumerable<EventType>> GetEventTypesWithEventsAsync(int filterByEventTypeId = 0, int page = 1, int pageSize = 25)
        {
            return await EventManagerContext.EventTypes
                .Where(x => (filterByEventTypeId == 0 || x.Id == filterByEventTypeId))
                .Skip((page-1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Events)
                .ToListAsync();
        }
        public SelectList GetEventTypeSelectList()
        {
            return new SelectList(EventManagerContext.EventTypes, nameof(EventType.Id), nameof(EventType.EventTypeName));
        }
    }
}
