using EventManager.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EventManager.Models.Domain
{
    public class EventSeries
    {
        private EventSeries() { }
        public EventSeries(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Event series title cannot be empty string", nameof(title));
            }
            else if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Event series description cannot be empty string", nameof(description));
            }
            else
            {
                Title = title;
                Description = description;
            }
            _events = new List<Event>();
        }
        
        public int Id { get; set; }
        public string Title { get;set;}
        public string Description { get; set;}
        private ICollection<Event> _events;
        public IEnumerable<Event> Events => _events.ToList();

        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                throw new ArgumentException("Event Series title cannot be empty string", nameof(newTitle));
            }
            else
            {
                Title = newTitle;
            }
        }
        public void UpdateDescription(string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
            {
                throw new ArgumentException("Event Series description cannot be empty string", nameof(newDescription));
            }
            else
            {
                Description = newDescription;
            }
        }
        public void AddEventToSeries(Event e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("Cannot add null Event to Event Series", nameof(e));
            }
            else
            {
                _events.Add(e);
            }
        }
        public void RemoveEventFromSeries(Event e)
        {
            if (e == null)
            {
                _events.Remove(e);
            }
        }
        public void AddEventRangeToSeries(IEnumerable<Event> events)
        {
            foreach (Event e in events)
            {
                AddEventToSeries(e);
            }            
        }
        public void RemoveEventRangeFromSeries(IEnumerable<Event> events)
        {
            foreach (Event e in events)
            {
                RemoveEventFromSeries(e);
            }
        }
        public IEnumerable<Event> GetAvailableEvents()
        {
            return Events?.Where(x => x.IsAcceptingRegistrations() == true).ToList() ?? new List<Event>();
        }
        public bool HasAvailableEvents()
        {
            return Events?.Any(x => x.IsAcceptingRegistrations() == true) ?? false;
        }
    }
}
