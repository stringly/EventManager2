using EventManager.Data.Core.Repositories;
using EventManager.Data.Core.Services;
using EventManager.Data.Persistence.Repositories;
using EventManager.Models.Domain;
using EventManager.Models.ViewModels;
using EventManager.sharedkernel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Persistence.Services
{
    public class EventService : IEventService
    {
        private readonly EventManagerContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _currentUserName;
        public IEventRepository Events { get; private set; }
        public IEventSeriesRepository EventSeries { get; private set; }
        public IEventTypeRepository EventTypes { get; private set; }
        public IRankRepository Ranks { get; private set; }
        public IUserRepository Users { get; private set; }
        public IRegistrationRepository Registrations { get; private set; }

        public EventService(EventManagerContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _currentUserName = _httpContextAccessor.HttpContext.User.Identity.Name.Split('\\')[1];
            Events = new EventRepository(_context);
            EventSeries = new EventSeriesRepository(_context);
            EventTypes = new EventTypeRepository(_context);
            Ranks = new RankRepository(_context);
            Users = new UserRepository(_context);
            Registrations = new RegistrationRepository(_context);
            
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public bool CreateEvent(EventAddViewModel form, out string response)
        {
            User creator = Users.GetUserByLDAPName(_currentUserName);
            if (creator == null)
            {
                response = $"User with {_currentUserName} could not be found when adding new Event.";
                return false;
            }
            EventType eventType = EventTypes.Get(form.EventTypeId);
            if (eventType == null)
            {
                response = $"Event Type with id {form.EventTypeId} could not be found when adding new Event.";
                return false;
            }
            Address location = Address.Create(form.AddressLine1, form.AddressLine2, form.City, form.State, form.Zip);
            Event eventToAdd = new Event(
                eventType,
                location,
                creator,
                form.Title,
                form.Description,
                form.StartDate,
                form.EndDate,
                form.RegistrationOpenDate,
                form.RegistrationClosedDate,
                form.MaxRegistrationCount,
                form.MinRegistrationCount,
                form.AllowStandby,
                form.MaxStandbyRegistrationCount,
                form.FundCenter
                );
            try
            {
                Events.Add(eventToAdd);
                Complete();
                response = "Event Added";
                return true;
            }
            catch (Exception e)
            {
                response = e.Message;
                return false;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
