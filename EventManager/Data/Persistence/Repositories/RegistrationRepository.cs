using EventManager.Data.Core.Repositories;
using EventManager.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Persistence.Repositories
{
    public class RegistrationRepository : Repository<Registration>, IRegistrationRepository
    {
        public EventManagerContext EventManagerContext {
            get { return context as EventManagerContext; }
        }
        public RegistrationRepository(EventManagerContext context)
            :base(context)
        { 
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsForUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsWithUserAndEvent(int filterByUserId = 0, int filterByEventId = 0, int filterByEventTypeId = 0, int page = 1, int pageSize = 25)
        {
            return await EventManagerContext.Registrations
                .Where(x => (filterByUserId == 0 || x.UserId == filterByUserId) && (filterByEventId == 0 || x.EventId == filterByEventId) && (filterByEventTypeId == 0 || x.Event.EventTypeId == filterByEventTypeId))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.User)
                    .ThenInclude(x => x.Rank)
                .Include(x => x.Event)
                    .ThenInclude(x => x.EventType)
                .ToListAsync();
        }
    }
}
