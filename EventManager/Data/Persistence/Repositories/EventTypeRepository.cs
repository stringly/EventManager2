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

        public async Task<IEnumerable<EventType>> GetEventTypesWithEventsAsync(string searchString = "", int page = 1, int pageSize = 25)
        {
            return await EventManagerContext.EventTypes
                .Where(x => (string.IsNullOrEmpty(searchString) || x.EventTypeName.ToLower().Contains(searchString)))
                .Skip((page-1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Events)
                .ToListAsync();
        }
        public async Task<EventType> GetEventTypeWithEventsAsync(int id)
        {
            return await EventManagerContext.EventTypes
                .Include(x => x.Events)
                    .ThenInclude(x => x.Owner)
                        .ThenInclude(x => x.Rank)
                .Include(x => x.Events)
                    .ThenInclude(x => x.Registrations)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public EventType GetEventTypeWithEvents(int id)
        {
            return EventManagerContext.EventTypes
                .Include(x => x.Events)
                    .ThenInclude(x => x.Owner)
                        .ThenInclude(x => x.Rank)
                .Include(x => x.Events)
                    .ThenInclude(x => x.Registrations)
                .FirstOrDefault(x => x.Id == id);
        }
        
        public SelectList GetEventTypeSelectList()
        {
            return new SelectList(EventManagerContext.EventTypes, nameof(EventType.Id), nameof(EventType.EventTypeName));
        }
        public async Task<SelectList> GetEventTypeSelectListAsync()
        {
            return new SelectList(await EventManagerContext.EventTypes.ToListAsync(), nameof(EventType.Id), nameof(EventType.EventTypeName));
        }
    }
}
