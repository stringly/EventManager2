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
        Task<IEnumerable<EventSeries>> GetEventSeriesWithEventsAsync(string searchString = "", int page = 1, int pageSize = 25);
        Task<EventSeries> GetEventSeriesWithEventsAndRegistrationsAsync(int id);
        EventSeries GetEventSeriesWithEventsAndRegistrations(int id);
        SelectList GetEventSeriesSelectList();
        Task<SelectList> GetEventSeriesSelectListAsync();
    }
}
