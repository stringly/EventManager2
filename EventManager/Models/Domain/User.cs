using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.Domain
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string LDAPName { get; set; }
        public int BlueDeckId { get;set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdNumber { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public int RankId { get; set; }
        public Rank Rank { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; }
        public virtual ICollection<Event> OwnedEvents { get; set; }

        [NotMapped]
        public string DisplayName => $"{Rank.ShortName} {FirstName} {LastName} {(String.IsNullOrEmpty(IdNumber) ? "" : $"#{IdNumber}")}";
    }
}
