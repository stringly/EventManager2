using EventManager.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.Domain
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RegistrationOpenDate { get; set; }
        public DateTime RegistrationClosedDate { get; set; }
        public int MinimumRegistrationsCount { get; set; }
        public int MaximumRegistrationsCount { get; set; }
        public bool StandbyRegistrationsAllowed { get; set; }
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
        public int? EventSeriesId { get; set; }
        public virtual EventSeries EventSeries { get; set;}
        public string CreatorId { get; set; }
        public virtual EventManagerUser Creator { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; }

        public bool IsOpenForRegistrations()
        {
            if (StartDate < DateTime.Now)
            {
                return false;
            }
            else if (RegistrationOpenDate > DateTime.Now || RegistrationClosedDate < DateTime.Now)
            {
                return false;
            }
            else if ()
        }

        public ICollection<Registration> ActiveRegistrations()
        {
            return Registrations.Where(x => x.IsActive).ToList();
        }
        public ICollection<Registration> StandbyRegistrations()
        {
            return Registrations.Where(x => x.IsStandy).ToList();
        }

        public ICollection<Registration> PendingRegistrations()
        {
            return Registrations.Where(x => x.IsPending).ToList();
        }
        public ICollection<Registration> RejectedRegistrations()
        {
            return Registrations.Where(x => x.IsRejected).ToList();
        }
    }
}
