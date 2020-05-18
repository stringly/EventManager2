using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class RegistrationAddViewModel
    {
        public RegistrationAddViewModel()
        {
        }
        [Display(Name = "Event"), Required]
        public int SelectedEventId { get; set; }
        [Display(Name = "User"), Required]
        public int SelectedUserId { get; set; }
        [Display(Name = "Status"), Required]
        public string SelectedRegistrationStatus { get; set;}
        public SelectList Events { get; set; }
        public SelectList Users { get; set; }
        public SelectList RegistrationStatuses { get; set; }
    }
}
