using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EventManager.Models.Domain
{
    /// <summary>
    /// Domain Entity Class that represents an Event for which Users can register
    /// </summary>
    public class Event
    {
        /// <summary>
        /// The Event's Identity Primary Key
        /// </summary>
        [Key]
        public int EventId { get; set; }
        /// <summary>
        /// The Title of the Event
        /// </summary>
        /// <remarks>
        /// The title is set by the event's creator and will be displayed in summary pages.
        /// </remarks>
        public string Title { get;set;}
        /// <summary>
        /// The Event's Description
        /// </summary>
        /// <remarks>
        /// A detailed text description of the event which is set by the event's creator
        /// </remarks>
        public string Description { get; set; }
        /// <summary>
        /// The Event's Fundcenter
        /// </summary>
        /// <remarks>
        /// A Fundcenter or other text identifier that is set by the event's creator. There are no stipulations on how this field is used; it can be given any text value to facilitate reporting
        /// </remarks>
        public string FundCenter { get; set; }
        /// <summary>
        /// The Event's Start Date
        /// </summary>
        /// <remarks>
        /// The date and time when the actual event is to begin.
        /// </remarks>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// The Event's End Date
        /// </summary>
        /// <remarks>
        /// The date and time when the actual event is to end.
        /// </remarks>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// The Event's Registration Period Open date
        /// </summary>
        /// <remarks>
        /// This field is set by the event's creator and will determine when the event will start to allow users to register to attend the event.
        /// </remarks>
        public DateTime RegistrationOpenDate { get; set; }
        /// <summary>
        /// The Event's Registration Period Closing Date
        /// </summary>
        /// <remarks>
        /// This field is set by the event's creator and will determine when the event will stop allowing users to register to attend.
        /// </remarks>
        public DateTime RegistrationClosedDate { get; set; }
        /// <summary>
        /// The Minimum number of attendees for the event
        /// </summary>
        /// <remarks>
        /// This field is optional and defaults to zero. It represents the minimum number of "Accepted" registrations allowed for the event.
        /// </remarks>
        public int MinimumRegistrationsCount { get; set; }
        /// <summary>
        /// The Maximum number of attendees for the event
        /// </summary>
        /// <remarks>
        /// This field is set by the event's creator and will set an upper limit on the number of "Accepted" registrations allowed for the event.
        /// </remarks>
        public int MaximumRegistrationsCount { get; set; }
        /// <summary>
        /// Boolean flag that determines whether the event is allowed to have "standby" users
        /// </summary>
        /// <remarks>
        /// This field is set by the event's creator and will determine whether automated ruleset registrations will allow "StandBy" registrations.
        /// </remarks>
        public bool StandbyRegistrationsAllowed { get; set; }
        /// <summary>
        /// The Maximum number of "Standby" registrations that are allowed.
        /// </summary>
        /// <remarks>
        /// This field is set by the event's creator and will determine the maximum number of automated ruleset "Standby" registrations are allowed.
        /// </remarks>
        public int MaximumStandbyRegistrationsCount { get; set; }
        public string LocationAddressLine1 { get; set; }
        public string LocationAddressLine2 { get; set; }
        public string LocationAddressCity { get; set; }
        public string LocationAddressState { get; set; }
        public string LocationAddressZip { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonPhone { get; set; }
        public string ContactPerson { get; set; }
        public int EventTypeId { get; set; }
        public virtual EventType EventType { get; set; }
        [NotMapped]
        public string EventTypeName { get {
                return EventType?.EventTypeName ?? "UNKNOWN";
            } 
        }
        public int? EventSeriesId { get; set; }
        public virtual EventSeries EventSeries { get; set;}
        public string CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; }

        public bool IsOpenForRegistrations()
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

        public ICollection<Registration> ActiveRegistrations()
        {
            return Registrations?.Where(x => x.IsActive).ToList() ?? new List<Registration>();
        }
        public ICollection<Registration> StandbyRegistrations()
        {
            return Registrations?.Where(x => x.IsStandy).ToList() ?? new List<Registration>();
        }

        public ICollection<Registration> PendingRegistrations()
        {
            return Registrations?.Where(x => x.IsPending).ToList() ?? new List<Registration>();
        }
        public ICollection<Registration> RejectedRegistrations()
        {
            return Registrations?.Where(x => x.IsRejected).ToList() ?? new List<Registration>();
        }
    }
}
