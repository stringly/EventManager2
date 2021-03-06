﻿using EventManager.sharedkernel;
using System;
using System.Collections.Generic;
using System.Linq;
using static EventManager.Models.Domain.Registration;

namespace EventManager.Models.Domain
{
    /// <summary>
    /// Domain Entity Class that represents an Event for which Users can register
    /// </summary>
    public class Event : IEntity
    {
        private Event()
        {
        }
        public Event(
            EventType type, 
            Address location,
            User owner,
            EventSeries series,
            string title, 
            string description,              
            DateTime startDate, 
            DateTime endDate, 
            DateTime registrationOpenDate,
            DateTime? registrationClosedDate = null,
            int maxRegistrations = 1,
            int minRegistrations = 0,
            bool allowStandbyRegistrations = false,
            int maxStandbyRegistrations = 0,
            string fundCenter = ""             
            )
        {
            UpdateEventType(type);
            UpdateEventLocation(location);
            UpdateOwner(owner);
            if(series == null)
            {
                RemoveEventFromSeries();
            }
            else
            {
                AddEventToSeries(series);
            }
            UpdateTitle(title);
            UpdateDescription(description);
            UpdateEventDates(startDate, endDate);
            UpdateRegistrationPeriodDates(registrationOpenDate, registrationClosedDate);
            if(maxRegistrations > 0)
            {
                UpdateMaximumRegistrationsAllowedCount((uint)maxRegistrations);
            }
            else
            {
                UpdateMaximumRegistrationsAllowedCount(1);
            }            
            UpdateMinimumRegistrationRequiredCount((uint)minRegistrations);
            if (allowStandbyRegistrations && maxStandbyRegistrations > 0)
            {
                AllowStandByRegistrations((uint)maxStandbyRegistrations);
            }
            else
            {
                PreventStandByRegistrations();
            }
            UpdateFundCenter(fundCenter);
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
        public IEnumerable<Registration> Registrations => _registrations?.ToList() ?? null;
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
            if (!String.IsNullOrWhiteSpace(newDescription))
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
        public void UpdateEventDates(DateTime? newStartDate = null, DateTime? newEndDate = null)
        {
            if(newStartDate == null && newEndDate == null)
            {
                return;
            }
            else if (newStartDate != null && newEndDate == null)
            {
                // User is changing only the Event Start, so check the new Start Date against the property current value
                if (newStartDate > EndDate)
                {
                    throw new ArgumentException("Event Start Date cannot be after End Date.", nameof(newStartDate));
                }
            }
            else if (newStartDate == null && newEndDate != null)
            {
                // User is changing only the Event End date, so check the new End Date against the property current value
                if (newEndDate < StartDate)
                {
                    throw new ArgumentException("Event End Date cannot precede Start Date.", nameof(newEndDate));
                }
            }
            else
            {
                // User is Changing both dates, so check the dates against each other
                if(newStartDate > newEndDate)
                {
                    throw new ArgumentException("Event End Date cannot precede Start Date.", nameof(newEndDate));
                }
                else
                {
                    // all conditions pass, set the Properties to the new value
                    StartDate = Convert.ToDateTime(newStartDate);
                    EndDate = Convert.ToDateTime(newEndDate);
                }
            }
        }
        public void UpdateRegistrationPeriodDates(DateTime? newStartDate, DateTime? newEndDate)
        {
            if (newStartDate == null && newEndDate == null)
            {
                return;
            }
            else if (newStartDate != null && newEndDate == null)
            {
                // User is changing only the Registration Period Start, so check the new Start Date against the property current value
                if (newStartDate > StartDate)
                {
                    throw new ArgumentException("Registration Period Start Date cannot be after Event Start Date.", nameof(newStartDate));
                }
                else if(newStartDate > RegistrationClosedDate)
                {
                    throw new ArgumentException("Registration Period Start Date cannot be after current Registration Period End Date.", nameof(newStartDate));
                }
                RegistrationOpenDate = Convert.ToDateTime(newStartDate);
                
            }
            else if (newStartDate == null && newEndDate != null)
            {
                // User is changing only the Registration Period End date, so check the new End Date against the property current value
                if (newEndDate < RegistrationOpenDate)
                {
                    throw new ArgumentException("Registration Period End Date cannot precede Registration Period Start Date.", nameof(newEndDate));
                }
                else if(newEndDate > EndDate){
                    throw new ArgumentException("Registration Period End Date cannot be after Event End Date.", nameof(newEndDate));
                }
                RegistrationClosedDate = Convert.ToDateTime(newEndDate);
            }
            else
            {
                // User is Changing both dates, so check the dates against each other
                if (newStartDate > newEndDate)
                {
                    throw new ArgumentException("Registration Period End Date cannot precede Registration Period Start Date.", nameof(newEndDate));
                }
                else if(newStartDate > StartDate)
                {
                    throw new ArgumentException("Registration Period Start Date cannot be after Event Start Date.", nameof(newEndDate));
                }
                else if(newEndDate > EndDate)
                {
                    throw new ArgumentException("Registration Period End Date cannot be after Event End Date.", nameof(newEndDate));
                }
                else
                {
                    // all conditions pass, set the Properties to the new value
                    RegistrationOpenDate = Convert.ToDateTime(newStartDate);
                    RegistrationClosedDate = Convert.ToDateTime(newEndDate);
                }
            }
        }
        public void UpdateMinimumRegistrationRequiredCount(uint newCount)
        {
            if (newCount > MaximumRegistrationsCount)
            {
                throw new ArgumentException("An Event cannot have more Minimum required Registrations than it's Maximum registration count.");
            }
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
        public void AddEventToSeries(EventSeries es)
        {
            if (es == null)
            {
                throw new ArgumentNullException("Cannot assign Event to null Event Series object.", nameof(es));
            }
            else
            {
                EventSeriesId = es.Id;
            }
        }
        public void RemoveEventFromSeries()
        {
            EventSeriesId = null;
        }
        public void UpdateOwner(User owner)
        {
            if(owner == null)
            {
                throw new ArgumentNullException("Cannot assign Event to null User.", nameof(owner));
            }
            OwnerId = owner.Id;
        }
        public bool IsAcceptingRegistrations()
        {
            EnsureRegistrationsLoaded();
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
            else if (Registrations.Count(x => x.IsActive == true) >= MaximumRegistrationsCount)
            { 
                return false;
            }
            else 
            {
                return true;
            }
        }
        public string GetEventStatus()
        {
            EnsureRegistrationsLoaded();
            if (IsAcceptingRegistrations())
            {
                return "Accepting Registrations";
            }
            else if(EndDate < DateTime.Now)
            {
                return "Expired";
            }
            else if (StartDate < DateTime.Now && EndDate > DateTime.Now)
            {
                return "In progress";
            }
            else if (RegistrationOpenDate > DateTime.Now)
            {
                return "Registration Period Pending";
            }
            else if(RegistrationClosedDate < DateTime.Now && StartDate > DateTime.Now)
            {
                return "Event Pending/Registrations closed.";
            }
            else if(StartDate > DateTime.Now && Registrations.Count() == MaximumRegistrationsCount)
            {
                return "Event Pending/Registrations Full";
            }
            else
            {
                return "Unknown";
            }
        }
        public IEnumerable<Registration> GetActiveRegistrations()
        {
            EnsureRegistrationsLoaded();
            return Registrations.Where(x => x.IsActive).ToList();
        }
        public IEnumerable<Registration> GetStandbyRegistrations()
        {
            EnsureRegistrationsLoaded();
            return Registrations.Where(x => x.IsStandy).ToList();
        }

        public IEnumerable<Registration> GetPendingRegistrations()
        {
            EnsureRegistrationsLoaded();
            return Registrations.Where(x => x.IsPending).ToList();
        }
        public IEnumerable<Registration> GetRejectedRegistrations()
        {
            EnsureRegistrationsLoaded();
            return Registrations.Where(x => x.IsRejected).ToList();
        }
        public bool AddRegistration(User u, RegistrationStatus status, out string response)
        {
            EnsureRegistrationsLoaded();
            Registration foundRegistration = GetRegistrationFromEventRegistrationsByUserId(u.Id);
            if (foundRegistration == null)
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
        public bool RemoveRegistration(int registrationId, out string response)
        {
            EnsureRegistrationsLoaded();
            Registration foundRegistration = GetRegistrationFromEventRegistrationsByRegistrationId(registrationId);
            if (foundRegistration != null)
            {
                _registrations.Remove(foundRegistration);
                response = "Registration removed from event.";
                return true;
            }
            else
            {
                response = $"Registration was not found for this event";
                return false;
            }
        }
        public bool AcceptRegistration(int registrationId, out string response)
        {
            //EnsureRegistrationsLoaded();
            throw new NotImplementedException();
        }
        public void EnsureRegistrationsLoaded()
        {
            if(_registrations == null)
            {
                throw new NullReferenceException("Registration collection is not loaded for this Event.");
            }

        }
        private Registration GetRegistrationFromEventRegistrationsByRegistrationId(int registrationId)
        {
            return _registrations.Where(x => x.Id == registrationId).FirstOrDefault();
        }
        private Registration GetRegistrationFromEventRegistrationsByUserId(int userId)
        {
            return _registrations.Where(x => x.UserId == userId).FirstOrDefault();
        }
    }
}
