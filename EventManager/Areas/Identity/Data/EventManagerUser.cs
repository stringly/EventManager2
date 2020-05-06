using EventManager.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Areas.Identity.Data
{
    public class EventManagerUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        public Rank Rank { get; set; }
        public string IdNumber { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; }
        public virtual ICollection<Event> OwnedEvents { get; set; }

        [NotMapped]
        public string DisplayName => $"{Rank.ShortName} {FirstName} {LastName} {(String.IsNullOrEmpty(IdNumber) ? "" : $"#{IdNumber}")}";
    }
}
