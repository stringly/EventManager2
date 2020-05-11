using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class EventTypeIndexViewModel : IndexViewModel
    {
        public string TypeNameSortOrder { get; set; }
        public string TypeEventCountSortOrder { get; set; }

        public List<EventTypeIndexViewModelEventTypeItem> EventTypes { get; private set;}
        public EventTypeIndexViewModel(int pageSize = 25)
        {
            PagingInfo = new PagingInfo { ItemsPerPage = pageSize };
        }
        public void InitializeEventTypeList(List<EventType> eventTypes, int page)
        {
            PagingInfo.TotalItems = eventTypes.Count();
            PagingInfo.CurrentPage = page;
            EventTypes = eventTypes
                .Skip((PagingInfo.CurrentPage - 1) * PagingInfo.ItemsPerPage)
                .Take(PagingInfo.ItemsPerPage)
                .ToList()                
                .ConvertAll(x => new EventTypeIndexViewModelEventTypeItem(x));            
        }

    }
    public class EventTypeIndexViewModelEventTypeItem
    {
        public int EventTypeId { get; set; }
        public string EventTypeName { get; set; }
        public int EventCount { get; set; }
        public EventTypeIndexViewModelEventTypeItem(EventType e)
        {
            EventTypeId = e.Id;
            EventTypeName = e.EventTypeName;
            EventCount = e?.Events?.Count() ?? 0;
        }
    }
}
