using EventManager.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EventManager.Models.Domain
{
    public class EventType : IEntity
    {
        private EventType() { }
        public EventType(string eventTypeName)
        {
            UpdateEventTypeName(eventTypeName);
            _events = new List<Event>();
        }        
        public int Id { get; private set;}
        public string EventTypeName { get; private set; }
        private ICollection<Event> _events;
        public IEnumerable<Event> Events => _events?.ToList() ?? new List<Event>();
        public void UpdateEventTypeName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Cannot update Event Type with empty Type Name.", nameof(newName));
            }
            EventTypeName = newName;
        }
    }
}
