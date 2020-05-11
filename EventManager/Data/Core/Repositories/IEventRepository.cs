using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<IEnumerable<Event>> GetEventsWithCreatorEventTypeAndSeriesAsnyc(int filterByEventTypeId = 0, int filterByCreatorUserId = 0, int filterByEventSeriesId = 0);
    }
}
