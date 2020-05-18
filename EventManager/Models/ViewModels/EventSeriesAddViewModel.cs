using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class EventSeriesAddViewModel
    {
        public EventSeriesAddViewModel()
        {
        }
        public EventSeriesAddViewModel(EventSeries es)
        {
            Id = es.Id;
            Title = es.Title;
            Description = es.Description;
        }
        [Display(Name = "Series Id")]
        public int Id { get; set; }
        [Display(Name = "Series Title"), Required, StringLength(50)]
        public string Title { get; set; }
        [Display(Name = "Description"), Required]
        public string Description { get; set; }
    }    
}
