using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Repositories
{
    public interface IRegistrationRepository : IRepository<Registration>
    {
        Task<IEnumerable<Registration>> GetRegistrationsForUserId(int userId);
        Task<IEnumerable<Registration>> GetRegistrationsWithUserAndEvent(int filterByUserId = 0, int filterByEventId = 0, int filterByEventTypeId = 0);
        
    }
}
