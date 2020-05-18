using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Repositories
{
    public interface IRegistrationRepository : IRepository<Registration>
    {
        Task<IEnumerable<Registration>> GetRegistrationsForUserIdByUserIdAsync(int userId);
        IEnumerable<Registration> GetRegistrationsForUserByUserId(int userId);
        Task<IEnumerable<Registration>> GetRegistrationsWithUserAndEventAsync(string selectedStatus, int filterByUserId = 0, int filterByEventId = 0, int page = 1, int pageSize = 25);
        IEnumerable<Registration> GetRegistrationsWithUserAndEvent(string selectedStatus, int filterByUserId = 0, int filterByEventId = 0, int filterByEventTypeId = 0, int page = 1, int pageSize = 25);
        Task<IEnumerable<Registration>> GetRegistrationsForEventByEventIdAsync(int eventId);
        IEnumerable<Registration> GetRegistrationsForEventByEventId(int eventId);
        Registration GetRegistrationWithUserAndEventByRegistrationId(int registrationId);
        Task<Registration> GetRegistrationWithUserAndEventByRegistrationIdAsync(int registrationId);
        SelectList GetRegistrationStatuses();
    }
}
