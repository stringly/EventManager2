using EventManager.Data.Persistence;
using EventManager.Models.Domain;
using EventManager.Models.ViewModels.Validations;
using EventManager.sharedkernel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Rewrite.Internal.IISUrlRewrite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class EventAddViewModel
    {
        public EventAddViewModel() {
            DateTime defaultDate = DateTime.Now;
            StartDate = defaultDate;
            EndDate = defaultDate.AddHours(10);
            RegistrationOpenDate = defaultDate;
            RegistrationClosedDate = defaultDate;
            }
        public EventAddViewModel(Event e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("Cannot create viewmodel from null Event object", nameof(e));
            }
            else
            {
                Id = e.Id;
                EventTypeId = e.EventTypeId;
                EventSeriesId = e?.EventSeriesId ?? 0;
                Title = e.Title;
                Description = e.Description;
                FundCenter = e.FundCenter;
                StartDate = e.StartDate;
                EndDate = e.EndDate;
                RegistrationOpenDate = e.RegistrationOpenDate;
                RegistrationClosedDate = e.RegistrationClosedDate;
                MinRegistrationCount = (int)e.MinimumRegistrationsCount;
                MaxRegistrationCount = (int)e.MaximumRegistrationsCount;
                AllowStandby = e.StandbyRegistrationsAllowed;
                MaxStandbyRegistrationCount = (int)e.MaximumStandbyRegistrationsCount;
                AddressLine1 = e.AddressFactory.AddressLine1;
                AddressLine2 = e.AddressFactory.AddressLine2;
                City = e.AddressFactory.City;
                State = e.AddressFactory.State;
                Zip = e.AddressFactory.Zip;                
            }
        }
        [Display(Name = "Event Id")]
        public int Id { get; set; }
        [Display(Name = "Event Type"), Required]
        public int EventTypeId { get; set; }
        [Display(Name = "Event Series")]
        public int? EventSeriesId { get; set; }
        [Display(Name = "Title"), 
            Required(ErrorMessage = "Event Title is required"), 
            StringLength(50, ErrorMessage = "Event Title cannot be longer than 50 characters")]
        public string Title { get; set; }
        [Display(Name = "Description"), Required]
        public string Description { get; set; }
        [Display(Name = "Fund Center"), 
            StringLength(25, ErrorMessage = "Fund Center cannot be longer than 25 characters.")]
        public string FundCenter { get; set; }
        [Display(Name = "Start Date/Time"), 
            Required, 
            DateMustBeFuture]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date/Time"), 
            Required, 
            MustBeAfterDate("StartDate", ErrorMessage = "Event End Date/Time must be after Start Date/Time")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Registration Open Date"), 
            Required, 
            DateMustBeFuture]
        public DateTime RegistrationOpenDate { get; set; }
        [Display(Name = "Registration Closed Date"), 
            Required, 
            MustBeBeforeDate("StartDate", ErrorMessage = "Registration period Start Date cannot be after the Event's Start Date"), 
            MustBeAfterDate("RegistrationOpenDate", ErrorMessage = "Registration Period End Date cannot be before the Registration Period End Date")]
        public DateTime RegistrationClosedDate { get; set; }
        [Display(Name = "Min Registrations"), NumberMustBeLessThanNumber("MaxRegistrationCount", ErrorMessage = "Min Registrations must be less than Max Registrations")]
        public int? MinRegistrationCount { get; set; }
        [Display(Name = "Max Registrations")]
        public int MaxRegistrationCount { get; set; }
        [Display(Name = "Allow Standy Registrations")]
        public bool AllowStandby { get; set; }
        [Display(Name = "Max Standy Registrations"), 
            RequireIfRelatedFieldTrue("AllowStandby", ErrorMessage = "You must provide the maximum number of allowed standby registrations to allow standby")]
        public int? MaxStandbyRegistrationCount { get; set; }        
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
        public SelectList EventTypes { get; set; }
        public SelectList States { get; set; }
        public SelectList EventSerieses { get; set; }
    }

}
