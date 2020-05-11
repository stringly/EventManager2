using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Repositories
{
    public interface IEventTypeRepository : IRepository<EventType>
    {
        Task<IEnumerable<EventType>> GetEventTypesWithEventsAsync(int filterByEventTypeId = 0);
    }
}
