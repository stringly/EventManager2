using EventManager.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EventManager.Models.Domain
{
    public class EventSeries : IEntity
    {
        private EventSeries() { }
        public EventSeries(string title, string description)
        {
            UpdateTitle(title);
            UpdateDescription(description);
            _events = new List<Event>();
        }
        
        public int Id { get; private set; }
        public string Title { get; private set;}
        public string Description { get; private set;}
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
                e.AddEventToSeries(this);
            }
        }
        public void RemoveEventFromSeries(Event e)
        {
            if (e == null)
            {
                e.RemoveEventFromSeries();
            }
        }
        public void AddEventRangeToSeries(IEnumerable<Event> events)
        {
            foreach (Event e in events)
            {
                e.AddEventToSeries(this);
            }            
        }
        public void RemoveEventRangeFromSeries(IEnumerable<Event> events)
        {
            foreach (Event e in events)
            {
                e.RemoveEventFromSeries();
            }
        }
        public IEnumerable<Event> GetAvailableEvents()
        {
            EnsureEventsLoaded();
            return Events.Where(x => x.IsAcceptingRegistrations() == true).ToList();
        }
        public bool HasAvailableEvents()
        {
            EnsureEventsLoaded();
            return Events.Any(x => x.IsAcceptingRegistrations() == true);
        }
        private void EnsureEventsLoaded()
        {
            if (Events == null)
            {
                throw new NullReferenceException("Event Series has null Events collection.");
            }
        }
    }
}
