using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class EventDetailsViewModel
    {
        public EventDetailsViewModel()
        {
        }
        public EventDetailsViewModel(Event e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("Cannot create viewmodel from null Event object", nameof(e));
            }
            else
            {
                Id = e.Id;
                EventTypeName = e?.EventType?.EventTypeName ?? "Unknown";
                EventSeriesId = e?.EventSeriesId ?? 0;
                EventSeriesName = e?.EventSeries?.Title ?? "";
                CreatorName = e?.Owner?.DisplayName ?? "Unknown";
                CreatorEmail = e?.Owner?.Email ?? "Unknown";
                Title = e.Title;
                Description = e.Description;
                FundCenter = e.FundCenter;
                StartDate = e.StartDate.ToString("MM/dd/yy HH:mm");
                EndDate = e.EndDate.ToString("MM/dd/yy HH:mm");
                RegistrationOpenDate = e.RegistrationOpenDate.ToString("MM/dd/yy HH:mm");
                RegistrationClosedDate = e.RegistrationClosedDate.ToString("MM/dd/yy HH:mm");
                MinRegistrationCount = (int)e.MinimumRegistrationsCount;
                MaxRegistrationCount = (int)e.MaximumRegistrationsCount;
                AllowStandby = e.StandbyRegistrationsAllowed;
                MaxStandbyRegistrationCount = (int)e.MaximumStandbyRegistrationsCount;
                AddressLine1 = e.AddressFactory.AddressLine1;
                AddressLine2 = e.AddressFactory.AddressLine2;
                City = e.AddressFactory.City;
                State = e.AddressFactory.State;
                Zip = e.AddressFactory.Zip;
                TotalRegistrations = e?.Registrations?.Count() ?? 0;
                AcceptedRegistrations = e?.Registrations.Where(x => x.IsActive)?.Count() ?? 0;
                PendingRegistrations = e?.Registrations.Where(x => x.IsPending)?.Count() ?? 0;
                StandbyRegistrations = e?.Registrations.Where(x => x.IsStandy)?.Count() ?? 0;
                RejectedRegistrations = e?.Registrations.Where(x => x.IsRejected)?.Count() ?? 0;
            }
        }
        [Display(Name = "Event Id")]
        public int Id { get; set; }
        [Display(Name = "Event Type")]
        public string EventTypeName { get; set; }
        [Display(Name = "Event Series")]
        public int EventSeriesId { get; set; }
        [Display(Name = "Event Series")]
        public string EventSeriesName { get; set;}
        [Display(Name = "Created By")]
        public string CreatorName { get; set;}
        [Display(Name = "Email")]
        public string CreatorEmail { get;set;}
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Fund Center")]
        public string FundCenter { get; set; }
        [Display(Name = "Start Date/Time")]
        public string StartDate { get; set; }
        [Display(Name = "End Date/Time")]
        public string EndDate { get; set; }
        [Display(Name = "Registration Open Date")]
        public string RegistrationOpenDate { get; set; }
        [Display(Name = "Registration Closed Date")]
        public string RegistrationClosedDate { get; set; }
        [Display(Name = "Min Registrations")]
        public int MinRegistrationCount { get; set; }
        [Display(Name = "Max Registrations")]
        public int MaxRegistrationCount { get; set; }
        [Display(Name = "Allow Standy Registrations")]
        public bool AllowStandby { get; set; }
        [Display(Name = "Max Standy Registrations")]
        public int MaxStandbyRegistrationCount { get; set; }
        [Display(Name = "Street Address"), Required, StringLength(50)]
        public string AddressLine1 { get; set; }
        [Display(Name = "Ste/Apt/Room#"), StringLength(25)]
        public string AddressLine2 { get; set; }
        [Display(Name = "City"), Required, StringLength(50)]
        public string City { get; set; }
        [Display(Name = "State"), Required]
        public string State { get; set; }
        [Display(Name = "ZIP Code"), Required, StringLength(5)]
        public string Zip { get; set; }
        [Display(Name = "Total")]
        public int TotalRegistrations { get; set; }
        [Display(Name = "Accepted")]
        public int AcceptedRegistrations { get; set; }
        [Display(Name = "Pending")]
        public int PendingRegistrations { get; set; }
        [Display(Name = "Standy")]
        public int StandbyRegistrations { get; set; }
        [Display(Name = "Rejected")]
        public int RejectedRegistrations { get; set; }
    }
}
