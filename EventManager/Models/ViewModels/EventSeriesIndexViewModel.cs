using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class EventSeriesIndexViewModel : IndexViewModel
    {
        public string TitleSort { get; set; }
        public string EventSeriesIdSort { get; set;}
        public List<EventSeriesIndexViewModelEventSeriesItem> EventSeriesItems { get; private set; }

        public EventSeriesIndexViewModel(int pageSize = 25)
        {
            PagingInfo = new PagingInfo { ItemsPerPage = 25 };
        }
        public void IntitializeEventSeriesList(List<EventSeries> eventSeries, int page)
        {
            PagingInfo.CurrentPage = page;
            PagingInfo.TotalItems = eventSeries.Count();
            EventSeriesItems = eventSeries
                .Skip((PagingInfo.CurrentPage - 1) * PagingInfo.ItemsPerPage)
                .Take(PagingInfo.ItemsPerPage)
                .ToList()
                .ConvertAll(x => new EventSeriesIndexViewModelEventSeriesItem(x));
            
        }
    }
    public class EventSeriesIndexViewModelEventSeriesItem
    {
        public int EventSeriesId { get; private set; }
        public string EventSeriesTitle { get; private set; }
        public int EventSeriesEventCount { get; private set; }

        public EventSeriesIndexViewModelEventSeriesItem(EventSeries e)
        {
            EventSeriesId = e.Id;
            EventSeriesTitle = e.Title;
            EventSeriesEventCount = e?.Events?.Count() ?? 0;
        }

    }
}
