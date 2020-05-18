using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class EventTypeAddViewModel
    {
        public EventTypeAddViewModel()
        {
        }
        public EventTypeAddViewModel(EventType et)
        {
            if (et == null)
            {
                throw new ArgumentNullException("Cannot create viewmodel from null EventType object", nameof(et));
            }
            EventTypeId = et.Id;
            EventTypeName = et.EventTypeName;
        }
        [Display(Name = "Event Type Id")]
        public int EventTypeId { get; set; }
        [Required, StringLength(25)]
        [Display(Name = "Event Type Name")]
        public string EventTypeName { get; set; }
    }
}
