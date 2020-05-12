using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Repositories
{
    public interface IEventSeriesRepository : IRepository<EventSeries>
    {
        Task<IEnumerable<EventSeries>> GetEventSeriesWithEventsAsync(int filterByEventSeriesId = 0, int page = 1, int pageSize = 25);
        SelectList GetEventSeriesSelectList();
    }
}
