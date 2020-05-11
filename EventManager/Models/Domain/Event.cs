using EventManager.sharedkernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventManager.Models.Domain
{
    /// <summary>
    /// Domain Entity Class that represents an Event for which Users can register
    /// </summary>
    public class Event
    {
        private Event()
        {
        }
        public Event(
            EventType type, 
            Address location,
            User creator,
            string title, 
            string description,              
            DateTime startDate, 
            DateTime endDate, 
            DateTime registrationOpenDate,
            DateTime? registrationClosedDate = null,
            uint maxRegistrations = 1,
            uint minRegistrations = 0,
            bool allowStandbyRegistrations = false,
            uint maxStandbyRegistrations = 0,
            string fundCenter = ""
            )
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Event title cannot be a null or empty string.", nameof(title));
            }
            else
            {
                _title = title;
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Event description cannot be a null or empty string.", nameof(description));
            }
            else
            {
                _description = description;
            }           
            if (startDate < DateTime.Now)
            {
                throw new ArgumentException("Event Start Date cannot be in the past.", nameof(startDate));
            }
            else
            {
                StartDate = startDate;
            } 
            if (endDate < startDate)
            {
                throw new ArgumentException("Event End Date cannot be before the Event Start Date.", nameof(endDate));
            }
            else
            {
                EndDate = endDate;
            }
            if (registrationOpenDate > startDate)
            {
                throw new ArgumentException("Registration Period Closed Open Date cannot be after Event Start Date.", nameof(registrationOpenDate));
            }
            else
            {
                RegistrationOpenDate = registrationOpenDate;
                
            }
            if (registrationClosedDate == null)
            {
                RegistrationClosedDate = startDate;
            }
            else
            {
                if (registrationClosedDate > startDate)
                {
                    throw new ArgumentException("Registration Period Closed date cannot be after Event Start Date.", nameof(registrationClosedDate));
                }
                else
                {
                    RegistrationClosedDate = Convert.ToDateTime(registrationClosedDate);
                }
            }
            
            if (type == null)
            {
                throw new ArgumentNullException("Requires Event Type of type EventManager.EventType.", nameof(type));
            }
            else
            {
                EventType = type;
            }
            if (creator == null)
            {
                throw new ArgumentNullException("Requires Creator of type EventManager.User.", nameof(creator));
            }
            else
            {
                Owner = creator;
            }
            MinimumRegistrationsCount = minRegistrations;
            MaximumRegistrationsCount = maxRegistrations;
            StandbyRegistrationsAllowed = allowStandbyRegistrations;
            MaximumStandbyRegistrationsCount = maxStandbyRegistrations;
            if(location == null || location.IsEmpty())
            {
                throw new ArgumentException("Address is null or empty.", nameof(location));
            }
            else
            {
                AddressFactory = location;
            }
            
            _fundCenter = fundCenter;
            _registrations = new List<Registration>();
     
        }
        /// <summary>
        /// The Event's Identity Primary Key
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The Title of the Event
        /// </summary>
        /// <remarks>
        /// The title is set by the event's creator and will be displayed in summary pages.
        /// </remarks>
        private string _title;
        public string Title => _title;

        /// <summary>
        /// The Event's Description
        /// </summary>
        /// <remarks>
        /// A detailed text description of the event which is set by the event's creator
        /// </remarks>
        private string _description;
        public string Description => _description;
        /// <summary>
        /// The Event's Fundcenter
        /// </summary>
        /// <remarks>
        /// A Fundcenter or other text identifier that is set by the event's creator. There are no stipulations on how this field is used; it can be given any text value to facilitate reporting
        /// </remarks>
        private string _fundCenter;
        public string FundCenter => _fundCenter;
        /// <summary>
        /// The Event's Start Date
        /// </summary>
        /// <remarks>
        /// The date and time when the actual event is to begin.
        /// </remarks>
        public DateTime StartDate { get; private set; }
        /// <summary>
        /// The Event's End Date
        /// </summary>
        /// <remarks>
        /// The date and time when the actual event is to end.
        /// </remarks>
        public DateTime EndDate { get; private set; }
        /// <summary>
        /// The Event's Registration Period Open date
        /// </summary>
        /// <remarks>
        /// This field is set by the event's creator and will determine when the event will start to allow users to register to attend the event.
        /// </remarks>
        public DateTime RegistrationOpenDate { get; private set; }
        /// <summary>
        /// The Event's Registration Period Closing Date
        /// </summary>
        /// <remarks>
        /// This field is set by the event's creator and will determine when the event will stop allowing users to register to attend.
        /// </remarks>
        public DateTime RegistrationClosedDate { get; private set; }
        /// <summary>
        /// The Minimum number of attendees for the event
        /// </summary>
        /// <remarks>
        /// This field is optional and defaults to zero. It represents the minimum number of "Accepted" registrations allowed for the event.
        /// </remarks>
        public uint MinimumRegistrationsCount { get; private set; }
        /// <summary>
        /// The Maximum number of attendees for the event
        /// </summary>
        /// <remarks>
        /// This field is set by the event's creator and will set an upper limit on the number of "Accepted" registrations allowed for the event.
        /// </remarks>
        public uint MaximumRegistrationsCount { get; private set; }
        /// <summary>
        /// Boolean flag that determines whether the event is allowed to have "standby" users
        /// </summary>
        /// <remarks>
        /// This field is set by the event's creator and will determine whether automated ruleset registrations will allow "StandBy" registrations.
        /// </remarks>
        public bool StandbyRegistrationsAllowed { get; private set; }
        /// <summary>
        /// The Maximum number of "Standby" registrations that are allowed.
        /// </summary>
        /// <remarks>
        /// This field is set by the event's creator and will determine the maximum number of automated ruleset "Standby" registrations are allowed.
        /// </remarks>
        public uint MaximumStandbyRegistrationsCount { get; private set; }
        /// <summary>
        /// Value object Address that represents the Event location
        /// </summary>
        public Address AddressFactory { get; private set; }
        /// <summary>
        /// Invokes the AddressFactory.FullAddress to show the Event's full address
        /// </summary>
        public string Address => AddressFactory.FullAddress;
        public int EventTypeId { get; private set; }
        public virtual EventType EventType { get; private set; }        
        public string EventTypeName => EventType?.EventTypeName ?? "UNKNOWN";
        public int? EventSeriesId { get; private set; }
        public virtual EventSeries EventSeries { get; private set;}
        public int OwnerId { get; private set; }
        public virtual User Owner { get; private set; }
        public string ContactPersonEmail => Owner?.Email ?? "";
        public string ContactPersonName => Owner?.Name ?? "";
        public string ContactPersonPhone => Owner?.ContactNumber ?? "";        

        /// <summary>
        /// Encapsulated collection of Registrations
        /// </summary>
        public IEnumerable<Registration> Registrations => _registrations.ToList();
        private ICollection<Registration> _registrations;

        public void UpdateTitle(string newTitle)
        {
            if (String.IsNullOrWhiteSpace(newTitle))
            {
                throw new ArgumentNullException("Event title cannot be a null or empty string.", nameof(newTitle));
            }
            else
            {
                _title = newTitle;
            }
        }
        public void UpdateDescription(string newDescription)
        {
            if (String.IsNullOrWhiteSpace(newDescription))
            {
                _description = newDescription;
            }
            else
            {
                throw new ArgumentNullException("Event description cannot be a null or empty string.", nameof(newDescription));
            }
        }
        public void UpdateFundCenter(string newFundCenter)
        {
            _fundCenter = newFundCenter;
        }
        public void UpdateStartDate(DateTime newStartDate)
        {
            if (newStartDate > EndDate)
            {
                throw new ArgumentException("Event Start Date cannot be after End Date.", nameof(newStartDate));
            }
            StartDate = newStartDate;
            if(RegistrationClosedDate < newStartDate)
            {
                RegistrationClosedDate = newStartDate;
            }
        }
        public void UpdateEndDate(DateTime newEndDate)
        {
            if(newEndDate < StartDate)
            {
                throw new ArgumentException("Event End Date cannot precede Start Date.", nameof(newEndDate));
            }
            else
            {
                EndDate = newEndDate;
            }            
        }
        public void UpdateRegistrationPeriodStartDate(DateTime newStartDate)
        {
            if (newStartDate > RegistrationClosedDate)
            {
                throw new ArgumentException("Event Registration Period Start Date cannot be after Registration Period End Date.", nameof(newStartDate));
            }
            else if(newStartDate > StartDate)
            {
                throw new ArgumentException("Event Registration Period Start Date cannot be after Event Start Date", nameof(newStartDate));
            }
            else if(newStartDate > EndDate)
            {
                throw new ArgumentException("Event Registration Period Start Date cannot be after Event End Date", nameof(newStartDate));
            }
            else
            {
                RegistrationOpenDate = newStartDate;
            }
        }
        public void UpdateRegistrationPeriodEndDate(DateTime newEndDate)
        {
            if (newEndDate > StartDate)
            {
                throw new ArgumentException("Event Registration Period End Date cannot be after Event End Date", nameof(newEndDate));
            }
            else if (newEndDate < RegistrationOpenDate)
            {
                throw new ArgumentException("Event Registration Period End Date cannot precede Registration Period Start Date.", nameof(newEndDate));
            }
            else
            {
                RegistrationClosedDate = newEndDate;
            }            
        }

        public void UpdateMinimumRegistrationRequiredCount(uint newCount)
        {
            MinimumRegistrationsCount = newCount;
        }
        public void UpdateMaximumRegistrationsAllowedCount(uint newCount)
        {
            if(newCount < MinimumRegistrationsCount)
            {
                throw new ArgumentException("Maximum registrations cannot be less than minimum registrations", nameof(newCount));
            }
            else
            {
                MaximumRegistrationsCount = newCount;
            }            
        }
        public void AllowStandByRegistrations(uint registrationsAllowedCount)
        {
            if(registrationsAllowedCount <= 0)
            {
                throw new ArgumentException("You must provide a number of allowed standby registrations greater than zero to allow standby registrations", nameof(registrationsAllowedCount));
            }
            else
            {
                StandbyRegistrationsAllowed = true;
                MaximumStandbyRegistrationsCount = registrationsAllowedCount;
            }
        }
        public void PreventStandByRegistrations()
        {
            StandbyRegistrationsAllowed = false;
        }
        public void UpdateEventLocation(Address newAddress)
        {
            if (newAddress == null || newAddress.IsEmpty())
            {
                throw new ArgumentException("Address is null or empty.", nameof(newAddress));
            }
            else
            {
                AddressFactory = newAddress;
            }
            
        }
        public void UpdateEventType(EventType type)
        {
            if (type == null)
            {
                throw new ArgumentException("Event Type must not be null.", nameof(type));
            }
            else
            {
                EventType = type;
            }
        }                
        public bool IsAcceptingRegistrations()
        {
            // Return false if Event Start Date is in the past
            if (StartDate < DateTime.Now)
            {
                return false;
            }
            // Return False if the Registration Period Start Date is in the future OR the Registration Period Close Date is in the past
            else if (RegistrationOpenDate > DateTime.Now || RegistrationClosedDate < DateTime.Now)
            {
                return false;
            }
            // Return false if the Maximum registration count has been reached
            else if (Registrations?.Count(x => x.IsActive == true) >= MaximumRegistrationsCount)
            { 
                return false;
            }
            else 
            {
                return true;
            }
        }

        public IEnumerable<Registration> GetActiveRegistrations()
        {
            return Registrations?.Where(x => x.IsActive).ToList() ?? new List<Registration>();
        }
        public IEnumerable<Registration> GetStandbyRegistrations()
        {
            return Registrations?.Where(x => x.IsStandy).ToList() ?? new List<Registration>();
        }

        public IEnumerable<Registration> GetPendingRegistrations()
        {
            return Registrations?.Where(x => x.IsPending).ToList() ?? new List<Registration>();
        }
        public IEnumerable<Registration> GetRejectedRegistrations()
        {
            return Registrations?.Where(x => x.IsRejected).ToList() ?? new List<Registration>();
        }
        public bool AddRegistration(User u, Registration.RegistrationStatus status, out string response)
        {
            if (_registrations == null)
            {
                response = "You must first retrieve this Event's existing list of registrations";
                return false;
            }
            var foundUserRegistration = _registrations.Where(x => x.UserId == u.Id).FirstOrDefault();
            if (foundUserRegistration == null)
            {
                _registrations.Add(new Registration(u, this, status));
                response = "Registration added to event.";
                return true;
            }
            else
            {
                response = "User is already registered for this event";
                return false;
            }
        }
        public bool RemoveRegistration(User u, out string response)
        {
            if (_registrations == null)
            {
                response = "You must first retrieve this Event's existing list of registrations";
                return false;
            }
            var foundUserRegistration = _registrations.Where(x => x.UserId == u.Id).FirstOrDefault();
            if (foundUserRegistration == null)
            {
                _registrations.Remove(foundUserRegistration);
                response = "Registration removed from event.";
                return true;
            }
            else
            {
                response = "User is not registered for this event";
                return false;
            }
        }
    }
}
