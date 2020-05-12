using EventManager.Models.Domain;
using EventManager.Models.ViewModels.Validations;
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
        public EventAddViewModel() { }
        [Display(Name = "Event Id")]
        public int Id { get; private set; }

        [Display(Name = "Title"), 
            Required(ErrorMessage = "Event Title is required"), 
            StringLength(50, ErrorMessage = "Event Title cannot be longer than 50 characters")]
        public string Title { get; private set; }
        [Display(Name = "Fund Center"), StringLength(25, ErrorMessage = "Fund Center cannot be longer than 25 characters.")]
        public string FundCenter { get; private set; }
        [Display(Name = "Start Date/Time"), Required, DateMustBeFuture]
        public DateTime StartDate { get; private set; }
        [Display(Name = "End Date/Time"), Required, MustBeAfterDate("StartDate", ErrorMessage = "Event End Date/Time must be after Start Date/Time")]
        public DateTime EndDate { get; private set; }
        [Display(Name = "Registration Open Date"), Required, DateMustBeFuture]
        public DateTime RegistrationOpenDate { get; private set; }
        [Display(Name = "Registration Closed Date"), 
            Required, 
            MustBeBeforeDate("StartDate", ErrorMessage = "Registration period Start Date cannot be after the Event's Start Date"), 
            MustBeAfterDate("RegistrationOpenDate", ErrorMessage = "Registration Period End Date cannot be before the Registration Period End Date")]
        public DateTime RegistrationClosedDate { get; private set; }

    }

}
