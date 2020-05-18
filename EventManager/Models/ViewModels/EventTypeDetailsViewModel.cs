using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class EventTypeDetailsViewModel
    {
        public EventTypeDetailsViewModel()
        {
        }
        public EventTypeDetailsViewModel(EventType et)
        {
            if (et == null)
            {
                throw new ArgumentNullException("Cannot create viewmodel from null EventType object", nameof(et));
            }
            else if (et.Events == null)
            {
                throw new ArgumentNullException("Cannot create viewmodel from Event Type object with null Events collection.", nameof(et.Events));
            }
            EventTypeId = et.Id;
            EventTypeName = et.EventTypeName;
            InitializeEventsCollection(et.Events);
        }
        [Display(Name = "Event Type Id"), HiddenInput]
        public int EventTypeId { get; set; }
        [Display(Name = "Event Type Name")]
        [Required, StringLength(25)]
        public string EventTypeName { get; set; }
        public IEnumerable<EventIndexViewModelEventItem> Events { get; private set; }
        public void InitializeEventsCollection(IEnumerable<Event> events)
        {
            Events = events.ToList().ConvertAll(x => new EventIndexViewModelEventItem(x));
        }
    }
}