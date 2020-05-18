using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class EventTypeIndexViewModel : IndexViewModel
    {
        public EventTypeIndexViewModel(
            IEnumerable<EventType> eventTypes, 
            string sortOrder, 
            string searchString, 
            int totalItems,
            int page = 1, 
            int pageSize = 25)
        {
            PagingInfo = new PagingInfo { 
                ItemsPerPage = pageSize,
                CurrentPage = page,
                TotalItems = totalItems
            };
            CurrentSort = sortOrder;
            CurrentFilter = searchString;
            EventTypes = eventTypes
                .ToList()
                .ConvertAll(x => new EventTypeIndexViewModelEventTypeItem(x));
            ApplySort(sortOrder);
            ApplyFilter(searchString);
        }
        public string TypeNameSortOrder { get; private set; }
        public string TypeEventCountSortOrder { get; private set; }

        public IEnumerable<EventTypeIndexViewModelEventTypeItem> EventTypes { get; private set;}

        private void ApplySort(string sortOrder)
        {
            switch (sortOrder)
            {
                case "eventTypeName_desc":
                    EventTypes = EventTypes.OrderByDescending(x => x.EventTypeName).ToList();
                    break;
                case "EventCount":
                    EventTypes = EventTypes.OrderBy(x => x.EventCount).ToList();
                    break;
                case "eventCount_desc":
                    EventTypes = EventTypes.OrderByDescending(x => x.EventCount).ToList();
                    break;
                default:
                    EventTypes = EventTypes.OrderBy(x => x.EventTypeName).ToList();
                    break;
            }
            TypeNameSortOrder = String.IsNullOrEmpty(sortOrder) ? "eventTypeName_desc" : "";
            TypeEventCountSortOrder = sortOrder == "EventCount" ? "eventCount_desc" : "EventCount";
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
                EventTypes = EventTypes
                    .Where(x => x.EventTypeName.ToLower().Contains(lowerString))
                    .ToList();
            }
        }

    }
    public class EventTypeIndexViewModelEventTypeItem
    {
        public int EventTypeId { get; private set; }
        public string EventTypeName { get; private set; }
        public int EventCount { get; private set; }
        public EventTypeIndexViewModelEventTypeItem(EventType e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("Cannot construct item from null EventType", nameof(e));
            }
            else if (e.Events == null)
            {
                throw new ArgumentNullException("Cannot construct item from EventType with null Events collection", nameof(e.Events));
            }
            else
            {
                EventTypeId = e.Id;
                EventTypeName = e.EventTypeName;
                EventCount = e.Events.Count();
            }            
        }
    }
}
