using EventManager.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.Domain
{
    public class Rank 
    {
        private Rank() { }
        public Rank(string abbreviation, string fullName)
        {
            if (string.IsNullOrWhiteSpace(abbreviation))
            {
                throw new ArgumentException("Rank Abbreviation cannot be empty string", nameof(abbreviation));
            }
            else if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Rank Full Name cannot be empty string", nameof(fullName));
            }
            else
            {
                Short = abbreviation;
                Full = fullName;
            }
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
    }
}
