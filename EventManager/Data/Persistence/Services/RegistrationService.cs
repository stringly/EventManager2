using EventManager.Data.Core.Repositories;
using EventManager.Data.Core.Services;
using EventManager.Data.Persistence.Repositories;
using EventManager.Models.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EventManager.Models.Domain.Registration;

namespace EventManager.Data.Persistence.Services
{
    public class RegistrationService : IRegistrationService
    {
        public RegistrationService(EventManagerContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            Events = new EventRepository(_context);
            Registrations = new RegistrationRepository(_context);
            Users = new UserRepository(_context);
            _currentUser = Users.GetUserByLDAPName(_httpContextAccessor.HttpContext.User.Identity.Name.Split('\\')[1]);
        }
        private readonly EventManagerContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly User _currentUser;
        public IEventRepository Events { get; private set; }
        public IRegistrationRepository Registrations { get; private set; }
        public IUserRepository Users { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public bool CreateRegistration(int userId, int eventId, out string response)
        {
            User u = Users.Get(userId);
            if(u == null)
            {
                response = $"Cannot create Registration: User with id {userId} was not found.";
                return false;
            }
            Event e = Events.GetEventWithRegistrations(eventId);
            try
            {
                e.EnsureRegistrationsLoaded();
            }
            catch (NullReferenceException ex)
            {
                response = ex.Message;
                return false;

            }
            if(e == null)
            {
                response = $"Cannot create Registration: Event with id {eventId} was not found.";
                return false; 
            }
            else if (!e.IsAcceptingRegistrations())
            {
                response = $"Cannot create Registration: Event is not accepting Registrations.";
                return false;
            }
            try
            {
                if(!e.AddRegistration(u, Registration.RegistrationStatus.Pending, out string addRegistrationToEventResponse)){
                    response = addRegistrationToEventResponse;
                    return false;
                }
                else
                {
                    Complete();
                    response = "Registration added.";
                    return true;
                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
                return false;
            }
        }

        public bool DeleteRegistration(int registrationId, out string response)
        {
            Registration r = Registrations.Get(registrationId);
            if (r == null)
            {
                response = $"Cannot remove Registration: No Registration with id {registrationId} was found.";
                return false;
            }
            Event e = Events.Get(r.EventId);
            if (e == null)
            {
                response = $"Cannot remove Registration: No Event with id {r.EventId} was found.";
                return false;
            }            
            else
            {
                EnsureRegistrationsLoadedForEvent(e);
                try
                {
                    if(!e.RemoveRegistration(r.Id, out string removeRegistrationFromEventResponse)){
                        response = removeRegistrationFromEventResponse;
                        return false;
                    }
                    else
                    {
                        Complete();
                        response = "Registration successfully removed.";
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    response = ex.Message;
                    return false;
                }
            }
        }

        public bool UpdateRegistrationStatus(int registrationId, RegistrationStatus status, out string response)
        {
            //Registration r = Registrations.Get(registrationId)
            throw new NotImplementedException();
        }
        private void EnsureRegistrationsLoadedForEvent(Event e)
        {
            try
            {
                e.EnsureRegistrationsLoaded();
            }
            catch
            {
                Registrations.GetRegistrationsForEventByEventId(e.Id);
                EnsureRegistrationsLoadedForEvent(e);
            }            
        }
    }
}
