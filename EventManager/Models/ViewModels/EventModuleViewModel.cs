using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class EventModuleViewModel
    {
        public EventModuleViewModel()
        {
            Deleted = false;
        }
        public EventModuleViewModel(EventModule em)
        {
            Id = em.Id;
            Title = em.Title;
            Description = em.Description;
            Deleted = false;
        }
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Title"), Required, StringLength(25)]
        public string Title { get; set; }
        [Display(Name = "Description"), Required]
        public string Description { get; set; }
        [HiddenInput]
        public bool Deleted { get; set; }
    }
}
