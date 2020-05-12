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
        public EventAddViewModel() {}
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
                MinRegistrationCount = e.MinimumRegistrationsCount;
                MaxRegistrationCount = e.MaximumRegistrationsCount;
                AllowStandby = e.StandbyRegistrationsAllowed;
                MaxStandbyRegistrationCount = e.MaximumStandbyRegistrationsCount;
                AddressLine1 = e.AddressFactory.AddressLine1;
                AddressLine2 = e.AddressFactory.AddressLine2;
                City = e.AddressFactory.City;
                State = e.AddressFactory.State;
                Zip = e.AddressFactory.Zip;                
            }
        }
        [Display(Name = "Event Id")]
        public int Id { get; private set; }
        [Display(Name = "Event Type"), Required]
        public int EventTypeId { get; private set; }
        [Display(Name = "Event Series")]
        public int EventSeriesId { get; private set; }
        [Display(Name = "Title"), 
            Required(ErrorMessage = "Event Title is required"), 
            StringLength(50, ErrorMessage = "Event Title cannot be longer than 50 characters")]
        public string Title { get; private set; }
        [Display(Name = "Description"), Required]
        public string Description { get; private set; }
        [Display(Name = "Fund Center"), 
            StringLength(25, ErrorMessage = "Fund Center cannot be longer than 25 characters.")]
        public string FundCenter { get; private set; }
        [Display(Name = "Start Date/Time"), 
            Required, 
            DateMustBeFuture]
        public DateTime StartDate { get; private set; }
        [Display(Name = "End Date/Time"), 
            Required, 
            MustBeAfterDate("StartDate", ErrorMessage = "Event End Date/Time must be after Start Date/Time")]
        public DateTime EndDate { get; private set; }
        [Display(Name = "Registration Open Date"), 
            Required, 
            DateMustBeFuture]
        public DateTime RegistrationOpenDate { get; private set; }
        [Display(Name = "Registration Closed Date"), 
            Required, 
            MustBeBeforeDate("StartDate", ErrorMessage = "Registration period Start Date cannot be after the Event's Start Date"), 
            MustBeAfterDate("RegistrationOpenDate", ErrorMessage = "Registration Period End Date cannot be before the Registration Period End Date")]
        public DateTime RegistrationClosedDate { get; private set; }
        [Display(Name = "Min Registrations")]
        public uint MinRegistrationCount { get; private set; }
        [Display(Name = "Max Registrations")]
        public uint MaxRegistrationCount { get; private set; }
        [Display(Name = "Allow Standy Registrations")]
        public bool AllowStandby { get; private set; }
        [Display(Name = "Max Standy Registrations"), 
            RequireIfRelatedFieldTrue("AllowStandby", ErrorMessage = "You must provide the maximum number of allowed standby registrations to allow standby")]
        public uint MaxStandbyRegistrationCount { get; private set; }        
        [Display(Name = "Street Address"), Required, StringLength(50)]
        public string AddressLine1 { get; private set; }
        [Display(Name = "Ste/Apt/Room#"), StringLength(25)]
        public string AddressLine2 { get; private set; }
        [Display(Name = "City"), Required, StringLength(50)]
        public string City { get; private set; }
        [Display(Name = "State"), Required]
        public string State { get; private set; }
        [Display(Name = "ZIP Code"), Required, StringLength(5)]
        public string Zip { get; private set; }                
        public SelectList EventTypes { get; set; }
        public SelectList States { get; set; }
        public SelectList EventSerieses { get; set; }
    }

}
