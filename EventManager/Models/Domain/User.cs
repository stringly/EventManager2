using EventManager.Data;
using EventManager.sharedkernel;
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
        private User() { }

        public User(string LDAPName, int blueDeckId, string firstName, string lastName, string idNumber, string email, string contactNumber, Rank rank)
        {
            _LDAPName = LDAPName;
            _blueDeckId = blueDeckId;
            _idNumber = idNumber;
            _email = email;
            _contactNumber = contactNumber;
            Rank = rank;
            NameFactory = PersonFullName.Create(firstName, lastName);
        }
        [Key]
        public int Id { get; private set; }
        private string _LDAPName;
        public string LDAPName => _LDAPName;
        private int _blueDeckId;
        public int BlueDeckId => _blueDeckId;
        public PersonFullName NameFactory { get; set; }
        public string Name=>NameFactory.FullName;
        private string _idNumber;
        public string IdNumber => _idNumber;
        private string _email;
        public string Email => _email;
        private string _contactNumber;
        public string ContactNumber => _contactNumber;
        public int? RankId { get; private set; }
        public Rank Rank { get; private set; }
        public IEnumerable<Registration> Registrations => _registrations.ToList();
        private ICollection<Registration> _registrations;
        public IEnumerable<Event> OwnedEvents => _ownedEvents.ToList();
        private ICollection<Event> _ownedEvents;

        [NotMapped]
        public string DisplayName => $"{Rank?.ShortName ?? ""} {Name} {(String.IsNullOrEmpty(IdNumber) ? "" : $"#{IdNumber}")}";
    }
}
