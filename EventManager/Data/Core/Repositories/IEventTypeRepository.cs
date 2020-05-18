using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Repositories
{
    public interface IEventTypeRepository : IRepository<EventType>
    {
        Task<IEnumerable<EventType>> GetEventTypesWithEventsAsync(string searchString = "", int page = 1, int pageSize = 25);
        Task<EventType> GetEventTypeWithEventsAsync(int id);
        EventType GetEventTypeWithEvents(int id);        
        SelectList GetEventTypeSelectList();
        Task<SelectList> GetEventTypeSelectListAsync();
    }
}
