using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class EventSeriesEditViewModel
    {
        public EventSeriesEditViewModel()
        {
            Events = new List<EventIndexViewModelEventItem>();
        }
        public EventSeriesEditViewModel(EventSeries es)
        {
            Id = es.Id;
            Title = es.Title;
            Description = es.Description;
            if (es.Events == null)
            {
                throw new ArgumentNullException("Cannot create viewmodel from EventSeries object with null Events collection.", nameof(es.Events));
            }
            else
            {
                InitializeEventList(es.Events);
            }
        }
        [Display(Name = "Series Id"), HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Series Title"), Required, StringLength(50)]
        public string Title { get; set; }
        [Display(Name = "Description"), Required]
        public string Description { get; set; }
        public IEnumerable<EventIndexViewModelEventItem> Events { get; private set; }
        public void InitializeEventList(IEnumerable<Event> events)
        {
            Events = events.ToList().ConvertAll(x => new EventIndexViewModelEventItem(x));
        }
    }

}