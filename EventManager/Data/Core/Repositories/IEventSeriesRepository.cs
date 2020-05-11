using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Repositories
{
    public interface IEventSeriesRepository : IRepository<EventSeries>
    {
        Task<IEnumerable<EventSeries>> GetEventSeriesWithEventsAsync(int filterByEventSeriesId = 0);
    }
}
