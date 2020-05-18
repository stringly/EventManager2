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
        public EventService(EventManagerContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;            
            Events = new EventRepository(_context);
            EventSeries = new EventSeriesRepository(_context);
            EventTypes = new EventTypeRepository(_context);
            Ranks = new RankRepository(_context);
            Users = new UserRepository(_context);
            Registrations = new RegistrationRepository(_context);
            _currentUser = Users.GetUserByLDAPName(_httpContextAccessor.HttpContext.User.Identity.Name.Split('\\')[1]);

        }
        private readonly EventManagerContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly User _currentUser;
        public IEventRepository Events { get; private set; }
        public IEventSeriesRepository EventSeries { get; private set; }
        public IEventTypeRepository EventTypes { get; private set; }
        public IRankRepository Ranks { get; private set; }
        public IUserRepository Users { get; private set; }
        public IRegistrationRepository Registrations { get; private set; }        

        public bool CreateEvent(
            out string response,
            int eventTypeId,
            int ownerUserId,            
            string title,
            string description,
            DateTime startDate,
            DateTime endDate,
            DateTime registrationOpenDate,
            DateTime? registrationClosedDate,
            string locationLine1,
            string locationLine2,
            string locationCity,
            string locationState,
            string locationZip,
            int eventSeriesId = 0,
            int maxRegistrations = 1,
            int minRegistrations = 0,
            bool allowStandbyRegistrations = false,
            int maxStandbyRegistrations = 0,
            string fundCenter = ""
            )
        {
            User creator = null;
            if (ownerUserId == 0)
            {
                creator = _currentUser;
            }
            else
            {
                creator = Users.Get(ownerUserId);
            }
            EventType eventType = EventTypes.Get(eventTypeId);
            EventSeries eventSeries = null;
            if(eventSeriesId != 0)
            {
                eventSeries = EventSeries.Get(eventSeriesId);
            }
            Address location = Address.Create(locationLine1, locationLine2, locationCity, locationState, locationZip);

            try
            {
                Event eventToAdd = new Event(
                    eventType,
                    location,
                    creator,
                    eventSeries,
                    title,
                    description,
                    startDate,
                    endDate,
                    registrationOpenDate,
                    registrationClosedDate,
                    maxRegistrations,
                    minRegistrations,
                    allowStandbyRegistrations,
                    maxStandbyRegistrations,
                    fundCenter
                    );
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

        public bool UpdateEvent(
            out string response,
            int eventId,
            int eventTypeId,
            int ownerUserId,            
            string title,
            string description,
            DateTime startDate,
            DateTime endDate,
            DateTime registrationOpenDate,
            DateTime? registrationClosedDate,
            string locationLine1,
            string locationLine2,
            string locationCity,
            string locationState,
            string locationZip,
            int eventSeriesId = 0,
            int maxRegistrations = 1,
            int minRegistrations = 0,
            bool allowStandbyRegistrations = false,
            int maxStandbyRegistrations = 0,
            string fundCenter = ""
            )
        {
            Event e = Events.Get(eventId);
            if(e == null)
            {
                throw new Exception($"No event with id {eventId} was found.");
            }
            try
            {
                if (e.Title != title) { e.UpdateTitle(title); }
                if (e.Description != description) { e.UpdateDescription(description); }
                if (e.FundCenter != fundCenter) { e.UpdateFundCenter(fundCenter); }
                if (e.StartDate != startDate && e.EndDate != endDate)
                {
                    e.UpdateEventDates(startDate, endDate);
                }
                else if(e.StartDate != startDate && e.EndDate == endDate)
                {
                    e.UpdateEventDates(startDate, null);
                }
                else if(e.StartDate == startDate && e.EndDate != endDate)
                {
                    e.UpdateEventDates(null, endDate);
                }

                if (e.RegistrationOpenDate != registrationOpenDate && e.RegistrationClosedDate != registrationClosedDate)
                {
                    e.UpdateRegistrationPeriodDates(registrationOpenDate, registrationClosedDate);
                }
                else if (e.RegistrationOpenDate != registrationOpenDate && e.RegistrationClosedDate == registrationClosedDate)
                {
                    e.UpdateRegistrationPeriodDates(registrationOpenDate, null);
                }
                else if (e.RegistrationOpenDate == registrationOpenDate && e.RegistrationClosedDate != registrationClosedDate)
                {
                    e.UpdateRegistrationPeriodDates(null, registrationClosedDate);
                }
                if (e.MinimumRegistrationsCount != minRegistrations) { e.UpdateMinimumRegistrationRequiredCount((uint)minRegistrations); }
                if (e.MaximumRegistrationsCount != maxRegistrations) { e.UpdateMaximumRegistrationsAllowedCount((uint)maxRegistrations); }
            
            
                if (allowStandbyRegistrations == false)
                {
                    e.PreventStandByRegistrations();
                    // TODO: EventService/UpdateEvent: Handle existing standby registrations when true => false
                }
                else
                {
                    e.AllowStandByRegistrations((uint)maxStandbyRegistrations);
                }
                // TODO: EventService/UpdateEvent: Handle null location?
                Address newLocation = Address.Create(locationLine1, locationLine2, locationCity, locationState, locationZip);
                if (e.AddressFactory != newLocation) { e.UpdateEventLocation(newLocation); }
                if (e.EventTypeId != eventTypeId) { e.UpdateEventType(EventTypes.Get(eventTypeId)); }            
                if (eventSeriesId != 0 && e.EventSeriesId != eventSeriesId)
                {
                    e.AddEventToSeries(EventSeries.Get(eventSeriesId));
                }
                else if(eventSeriesId == 0 && e.EventSeriesId != null)
                {
                    e.RemoveEventFromSeries();
                }
                if(e.EventTypeId != eventTypeId)
                {
                    e.UpdateEventType(EventTypes.Get(eventTypeId));
                }
                if(ownerUserId != 0 && e.OwnerId != ownerUserId)
                {
                    e.UpdateOwner(Users.Get(ownerUserId));
                }
                Complete();
                response = "Event Updated";
                return true;
            }
            catch(Exception ex)
            {
                response = ex.Message;
                return false;
            }                
        }
        public bool DeleteEvent(int id, out string response)
        {
            // TODO: EventService/Delete: Notification options if there are registrations?
            try
            {
                // TODO: EventService/Delete: Will this cascade to delete Registrations?                
                Event toRemove = Events.Get(id);
                Events.Remove(toRemove);
                Complete();
                response = "Event Deleted.";
                return true;
            }
            catch(Exception ex)
            {
                response = $"Event could not be deleted: {ex.Message}";
                return false;
            }
        }        

        public bool CreateEventSeries(string title, string description, out string response)
        {
            if (EventSeries.ValueIsInUseByIdForExpression(x => x.Title == title))
            {
                response = $"An Event Series with the name {title} already exists.";
                return false;
            }
            try
            {
                EventSeries toAdd = new EventSeries(title, description);
                EventSeries.Add(toAdd);
                Complete();
                response = "Event Series added";
                return true;
            }
            catch (Exception ex)
            {
                response = ex.Message;
                return false;
            }
        }

        public bool UpdateEventSeries(int eventSeriesId, string title, string description, out string response)
        {
            EventSeries toUpdate = EventSeries.Get(eventSeriesId);            
            if (EventSeries.ValueIsInUseByIdForExpression(x => x.Title == title && x.Id != eventSeriesId))
            {
                response = $"Cannot update Event Series: Title is in use by another Series.";
                return false;
            }
            try
            {                
                toUpdate.UpdateTitle(title);
                toUpdate.UpdateDescription(description);                
                Complete();
                response = "Event Series updated.";
                return true;
            }
            catch (Exception ex)
            {
                response = ex.Message;
                return false;
            }
        }

        public bool DeleteEventSeries(int id, out string response)
        {
            try
            {
                EventSeries toRemove = EventSeries.Get(id);
                EventSeries.Remove(toRemove);
                List<Event> eventsForRemovedSeries = Events.Find(x => x.EventSeriesId == id).ToList();
                foreach (Event e in eventsForRemovedSeries)
                {
                    e.RemoveEventFromSeries();
                }
                Complete();
                response = $"Event Series removed, {eventsForRemovedSeries} Events reassigned.";
                return true;
            }
            catch (Exception ex)
            {
                response = ex.Message;
                return false;
            }
        }

        public bool CreateEventType(string eventTypeName, out string response)
        {
            if (EventTypes.ValueIsInUseByIdForExpression(x => x.EventTypeName == eventTypeName))
            {
                response = $"An Event Type with name {eventTypeName} already exists.";
                return false;
            }            
            try
            {
                EventType toAdd = new EventType(eventTypeName);
                EventTypes.Add(toAdd);
                Complete();
                response = "Event Type added.";
                return true;
            }
            catch (Exception ex)
            {
                response = ex.Message;
                return false;
            }
        }

        public bool UpdateEventType(int eventTypeId, string eventTypeName, out string response)
        {
            if (EventTypes.ValueIsInUseByIdForExpression(x => x.EventTypeName == eventTypeName && x.Id != eventTypeId))
            {
                response = $"An Event Type named {eventTypeName} already exists.";
                return false;
            }            
            try
            {
                EventType toUpdate = EventTypes.Get(eventTypeId);
                toUpdate.UpdateEventTypeName(eventTypeName);
                Complete();
                response = "Event Type added.";
                return true;
            }
            catch (Exception ex)
            {
                response = ex.Message;
                return false;
            }
        }

        public bool DeleteEventType(int id, out string response)
        {
            EventType toRemove = EventTypes.GetEventTypeWithEvents(id);
            if (toRemove == null)
            {
                response = $"Cannot delete Event Type: No Event Type with id {id} was found.";
                return false;
            }
            else if(toRemove.Events.Count() > 0)
            {
                response = $"Cannot delete Event Type: Event Type has Events assigned.";
                return false;
            }
            try
            {                
                EventTypes.Remove(toRemove);
                Complete();
                response = "Event Type deleted.";
                return true;
            }
            catch (Exception ex)
            {
                response = ex.Message;
                return false;
            }
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
