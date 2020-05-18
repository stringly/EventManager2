using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class RegistrationDetailsViewModel
    {
        public RegistrationDetailsViewModel() { }
        public RegistrationDetailsViewModel(Registration r)
        {
            if(r == null)
            {
                throw new ArgumentNullException("Cannot construct view model from null Registration object", nameof(r));
            }
            else if(r.User == null)
            {
                throw new ArgumentNullException("Cannot construct view model from Registration object with null User property", nameof(r.User));
            }
            else if(r.Event == null)
            {
                throw new ArgumentNullException("Cannot construct view model from Registration object with null Event property", nameof(r.Event));
            }
            RegistrationId = r.Id;
            Status = r.Status.ToString();
            TimeStamp = r.TimeStamp.ToString("MM/dd/yy HH:mm");
            UserId = (int)r.UserId;
            UserName = r.UserDisplayName;
            Email = r.UserEmail;
            Phone = r.UserContactNumber;
            EventId = r.EventId;
            EventName = r.Event.Title;
            EventDate = r.Event.StartDate.ToString("MM/dd/yy");
        }
        [HiddenInput]
        public int RegistrationId { get;set;}        
        [Display(Name = "Registration Date/Time")]
        public string TimeStamp { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        public int UserId { get; set; }
        [Display(Name = "User")]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        public int EventId { get; set; }
        [Display(Name ="Event Title")]
        public string EventName { get; set; }
        [Display(Name = "Event Date")]
        public string EventDate { get; set; }
        
    }
}
