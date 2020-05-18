using EventManager.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.Domain
{
    public class Rank : IEntity
    {
        private Rank() { }
        public Rank(string abbreviation, string fullName)
        {
            UpdateAbbreviation(abbreviation);
            UpdateFullName(fullName);
            _users = new List<User>();
        }
        public int Id { get; private set; }
        public string Short { get; private set; }
        public string Full { get; private set; }
        private ICollection<User> _users;
        public IEnumerable<User> Users => _users.ToList();
        
        public string FullName { get {
                if (Short != "N/A")
                {
                    return Full;
                }
                else
                {
                    return "Mr./Ms.";
                }
            } 
        }
        
        public string ShortName {
            get {
                if (Short != "N/A")
                {
                    return Short;
                }
                else
                {
                    return "Mr./Ms.";
                }
            }
        }
        public void UpdateAbbreviation(string newAbbrev)
        {
            if (string.IsNullOrWhiteSpace(newAbbrev))
            {
                throw new ArgumentException("Cannot update Rank Short Name to empty string.", nameof(newAbbrev));
            }
            Short = newAbbrev;
        }
        public void UpdateFullName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Cannot update Rank Short Name to empty string.", nameof(newName));
            }
            Full = newName;
        }
    }
}
