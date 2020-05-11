using EventManager.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EventManager.Models.Domain
{
    public class EventType
    {
        private EventType() { }
        public EventType(string eventTypeName)
        {
            if (string.IsNullOrWhiteSpace(eventTypeName))
            {
                throw new ArgumentException("Event Type name cannot be empty string", nameof(eventTypeName));
            }
            else
            {
                EventTypeName = eventTypeName;
            }
            _events = new List<Event>();
        }        
        public int Id { get; private set;}
        public string EventTypeName { get; private set; }
        private ICollection<Event> _events;
        public IEnumerable<Event> Events => _events?.ToList() ?? new List<Event>();
    }
}
