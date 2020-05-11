using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class EventIndexViewModel : IndexViewModel
    {
        public string UserIdSort { get; set; }
        public string EventTypeSort { get; set; }
        public string CreatedDateSort { get; set; }
        public string StartDateSort { get; set; }
        public string EventSeriesSort { get; set; }
        public int SelectedUserId { get; set; }
        public int SelectedEventTypeId { get; set; }
        
        public List<EventIndexViewModelEventItem> Events { get; private set;}

        public List<SelectListItem> Users { get; private set;}
        public List<SelectListItem> EventTypes { get; private set;}

        public EventIndexViewModel(int pageSize = 25)
        {            
            PagingInfo = new PagingInfo { ItemsPerPage = pageSize };            
        }
        public void InitializeEventList(List<Event> events, int page)
        {
            PagingInfo.CurrentPage = page;
            PagingInfo.TotalItems = events.Count();
            Events = events
                .Skip((PagingInfo.CurrentPage -1) * PagingInfo.ItemsPerPage)
                .Take(PagingInfo.ItemsPerPage)
                .ToList()
                .ConvertAll(x => new EventIndexViewModelEventItem(x));
            
            Users = events.Select(x => x.Creator).Distinct().ToList().ConvertAll(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString() });
            EventTypes = events.Select(x => x.EventType).Distinct().ToList().ConvertAll(x => new SelectListItem { Text = x.EventTypeName, Value = x.Id.ToString()});
        }

    }

    public class EventIndexViewModelEventItem
    {
        public int EventId { get; set; }
        public string CreatedDate { get; set;}
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string EventTitle {get; set; }
        public string EventSeriesTitle { get; set; }
        public int EventSeriesId { get; set; }
        public string CreatedByUserDisplayName { get; set; }
        public string CreatedByUserEmail { get; set; }
        public string EventTypeName { get; set; }
        public int CreatedByUserId { get; set; }
        public EventIndexViewModelEventItem(Event e)
        {
            EventId = e.Id;
            CreatedDate = e.CreatedDate.ToString("MM/dd/yy HH:mm");
            StartDate = e.StartDate.ToString("MM/dd/yy HH:mm");
            EndDate = e.EndDate.ToString("MM/dd/yy HH:mm");
            EventTitle = e.Title;
            EventSeriesTitle = e?.EventSeries?.Title ?? "-";
            CreatedByUserDisplayName = e?.Creator?.DisplayName ?? "-";
            CreatedByUserEmail = e?.Creator?.Email ?? "-";
            EventTypeName = e.EventTypeName;
            CreatedByUserId = e?.Creator?.Id ?? 0;
        }
    }
}
