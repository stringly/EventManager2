using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class EventSeriesIndexViewModel : IndexViewModel
    {
        public EventSeriesIndexViewModel(
            IEnumerable<EventSeries> eventSeries, 
            string sortOrder, 
            string searchString, 
            int page, 
            int pageSize = 25
            )
        {
            PagingInfo = new PagingInfo { 
                ItemsPerPage = pageSize, 
                CurrentPage = page,
                TotalItems = eventSeries.Count()
            };
            EventSeriesItems = eventSeries
                .ToList()
                .ConvertAll(x => new EventSeriesIndexViewModelEventSeriesItem(x));
            CurrentFilter = searchString;
            CurrentSort = sortOrder;
            ApplySort(sortOrder);
            ApplyFilter(searchString);
        }
        public string TitleSort { get; private set; }
        public string EventSeriesIdSort { get; private set;}
        public IEnumerable<EventSeriesIndexViewModelEventSeriesItem> EventSeriesItems { get; private set; }

        private void ApplySort(string sortOrder)
        {
            switch (sortOrder)
            {
                case "eventSeriesId_desc":
                    EventSeriesItems = EventSeriesItems.OrderBy(x => x.EventSeriesId).ToList();
                    break;
                case "EventTitle":
                    EventSeriesItems = EventSeriesItems.OrderBy(x => x.EventSeriesTitle).ToList();
                    break;
                case "eventTitle_desc":
                    EventSeriesItems = EventSeriesItems.OrderByDescending(x => x.EventSeriesTitle).ToList();
                    break;
                default:
                    EventSeriesItems = EventSeriesItems.OrderByDescending(x => x.EventSeriesId).ToList();
                    break;
            }
            EventSeriesIdSort = String.IsNullOrEmpty(sortOrder) ? "eventSeriesId_desc" : "";
            TitleSort = sortOrder == "EventTitle" ? "eventTitle_desc" : "EventTitle";
        }
        private void ApplyFilter(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                  || char.IsWhiteSpace(c)
                                  || c == '-')));
                string lowerString = new string(arr);
                lowerString = lowerString.ToLower();
                EventSeriesItems = EventSeriesItems
                    .Where(x => x.EventSeriesTitle.ToLower().Contains(lowerString))
                    .ToList();
            }
        }
        
    }
    public class EventSeriesIndexViewModelEventSeriesItem
    {
        public int EventSeriesId { get; private set; }
        public string EventSeriesTitle { get; private set; }
        public int EventSeriesEventCount { get; private set; }

        public EventSeriesIndexViewModelEventSeriesItem(EventSeries e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("Cannot construct item from null EventSeries parameter", nameof(e));
            }
            else if (e.Events == null)
            {
                throw new ArgumentNullException("Cannot construct item from Event with null Events Collection", nameof(e.Events));
            }
            else
            {
                EventSeriesId = e.Id;
                EventSeriesTitle = e.Title;
                EventSeriesEventCount = e?.Events?.Count() ?? 0;
            }
        }
    }
}
