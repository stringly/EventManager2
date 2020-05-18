using EventManager.Data.Core.Repositories;
using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<IEnumerable<Registration>> GetRegistrationsForUserIdByUserIdAsync(int userId)
        {
            return await EventManagerContext.Registrations
                .Where(x => x.UserId == userId)
                .Include(x => x.Event)
                    .ThenInclude(x => x.Owner)
                        .ThenInclude(x => x.Rank)
                .Include(x => x.Event)
                    .ThenInclude(x => x.EventType)
                .Include(x => x.User)
                .ToListAsync();
        }
        public IEnumerable<Registration> GetRegistrationsForUserByUserId(int userId)
        {
            return EventManagerContext.Registrations
                .Where(x => x.UserId == userId)
                .Include(x => x.Event)
                    .ThenInclude(x => x.Owner)
                        .ThenInclude(x => x.Rank)
                .Include(x => x.Event)
                    .ThenInclude(x => x.EventType)
                .Include(x => x.User)
                .ToList();
        }
        public async Task<IEnumerable<Registration>> GetRegistrationsWithUserAndEventAsync(string selectedStatus, int filterByUserId = 0, int filterByEventId = 0, int page = 1, int pageSize = 25)
        {
            return await EventManagerContext.Registrations
                .Where(x => (string.IsNullOrEmpty(selectedStatus) || x.Status.ToString() == selectedStatus) && (filterByUserId == 0 || x.UserId == filterByUserId) && (filterByEventId == 0 || x.EventId == filterByEventId))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.User)
                    .ThenInclude(x => x.Rank)
                .Include(x => x.Event)
                    .ThenInclude(x => x.EventType)
                .ToListAsync();
        }

        IEnumerable<Registration> IRegistrationRepository.GetRegistrationsWithUserAndEvent(string selectedStatus, int filterByUserId, int filterByEventId, int filterByEventTypeId, int page, int pageSize)
        {
            return EventManagerContext.Registrations
                .Where(x => (string.IsNullOrEmpty(selectedStatus) || x.Status.ToString() == selectedStatus) && (filterByUserId == 0 || x.UserId == filterByUserId) && (filterByEventId == 0 || x.EventId == filterByEventId) && (filterByEventTypeId == 0 || x.Event.EventTypeId == filterByEventTypeId))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.User)
                    .ThenInclude(x => x.Rank)
                .Include(x => x.Event)
                    .ThenInclude(x => x.EventType)
                .ToList();
        }

        public async Task<IEnumerable<Registration>> GetRegistrationsForEventByEventIdAsync(int eventId)
        {
            return await EventManagerContext.Registrations
                .Where(x => x.EventId == eventId)
                .ToListAsync();
        }

        public IEnumerable<Registration> GetRegistrationsForEventByEventId(int eventId)
        {
            return EventManagerContext.Registrations
                .Where(x => x.EventId == eventId)
                .ToList();
        }
        public Registration GetRegistrationWithUserAndEventByRegistrationId(int registrationId)
        {
            return EventManagerContext.Registrations
                .Include(x => x.User)
                    .ThenInclude(x => x.Rank)
                .Include(x => x.Event)
                    .ThenInclude(x => x.EventType)
                .FirstOrDefault();
        }

        public async Task<Registration> GetRegistrationWithUserAndEventByRegistrationIdAsync(int registrationId)
        {
            return await EventManagerContext.Registrations
                .Include(x => x.User)
                    .ThenInclude(x => x.Rank)
                .Include(x => x.Event)
                    .ThenInclude(x => x.EventType)
                .FirstOrDefaultAsync();
        }

        public SelectList GetRegistrationStatuses()
        {
            return new SelectList(new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Pending", Value = "Pending"},
                new SelectListItem() { Text = "Accepted", Value = "Accepted" },
                new SelectListItem() { Text = "Standby", Value = "Standby" },
                new SelectListItem() { Text = "Rejected", Value = "Rejected" }
            }, "Value", "Text");
        }


    }
}
