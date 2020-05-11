using EventManager.sharedkernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventManager.Models.Domain
{
    public class User
    {
        private User() { }

        public User(string LDAPName, uint blueDeckId, string firstName, string lastName, string idNumber, string email, string contactNumber, Rank rank)
        {

            _LDAPName = !string.IsNullOrWhiteSpace(LDAPName) ? LDAPName  : throw new ArgumentException("Cannot set LDAP Name to empty string", nameof(LDAPName));
            _blueDeckId = blueDeckId;
            _idNumber = idNumber;
            _email = !string.IsNullOrWhiteSpace(email) ? email : throw new ArgumentException("Cannot set Email to empty string", nameof(email));
            _contactNumber = contactNumber;
            Rank = rank != null ? rank : throw new ArgumentException("User Rank cannot be null", nameof(rank));
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("First name cannot be empty string", nameof(firstName));
            }
            else if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Last name cannot be empty string", nameof(lastName));
            }
            else
            {
                NameFactory = PersonFullName.Create(firstName, lastName);
            }
            _registrations = new List<Registration>();
            _ownedEvents = new List<Event>();
            
        }
        
        public int Id { get; private set; }
        private string _LDAPName;
        public string LDAPName => _LDAPName;
        private uint _blueDeckId;
        public uint BlueDeckId => _blueDeckId;
        public PersonFullName NameFactory { get; private set; }
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
        public string DisplayName => $"{Rank?.ShortName ?? ""} {Name} {(String.IsNullOrEmpty(IdNumber) ? "" : $"#{IdNumber}")}";

        public void UpdateLDAPName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Cannot set field to empty string", nameof(newName));
            }
            else
            {
                _LDAPName = newName;
            }            
        }
        public void UpdateBlueDeckId(uint newId)
        {
            _blueDeckId = newId;
        }
        public void UpdateName(PersonFullName newNameFactory)
        {
            if (newNameFactory == null || newNameFactory.IsEmpty())
            {
                throw new ArgumentException("Cannot set name with null or empty NameFactory", nameof(newNameFactory));
            }
            else
            {
                NameFactory = newNameFactory;
            }
        }
        public void UpdateIdNumber(string newId)
        {
            _idNumber = newId;
        }
        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrEmpty(newEmail))
            {
                throw new ArgumentException("Cannot set email to null or empty string", nameof(newEmail));
            }
        } 
        public void UpdateContactNumber(string newNumber)
        {
            _contactNumber = newNumber;                
        }
        public void UpdateRank(Rank newRank)
        {
            if (newRank == null)
            {
                throw new ArgumentException("Rank cannot be null", nameof(newRank));
            }
            else
            {
                Rank = newRank;
            }
        }

        public void AddOwnedEvent(Event eventToAdd)
        {
            if (eventToAdd == null)
            {
                throw new ArgumentException("Event cannot be null", nameof(eventToAdd));
            }
            else
            {
                _ownedEvents.Add(eventToAdd);
            }
        }
        public void RemoveOwnedEvent(Event eventToRemove)
        {
            if (eventToRemove == null)
            {
                throw new ArgumentException("Event cannot be null", nameof(eventToRemove));
            }
            else
            {
                _ownedEvents.Remove(eventToRemove);
            }
        }
    }
}
