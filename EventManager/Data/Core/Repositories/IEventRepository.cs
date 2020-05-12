using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<IEnumerable<Event>> GetEventsWithCreatorEventTypeAndSeriesAsnyc(int filterByEventTypeId = 0, int filterByCreatorUserId = 0, int filterByEventSeriesId = 0, int page = 1, int pageSize = 25);
        SelectList GetEventSelectList();
        
    }
}
