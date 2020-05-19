using EventManager.Models.Domain;
using EventManager.Models.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class EventIndexViewModel : IndexViewModel
    {
        private EventIndexViewModel() { }

        public EventIndexViewModel(
            IEnumerable<EventDto> events, 
            SelectList users, 
            SelectList eventTypes, 
            int selectedUserId, 
            int selectedEventTypeId,
            string sortOrder,
            string searchString,
            int totalItems = 0,
            int page = 1,
            int pageSize = 25)
        {

            PagingInfo = new PagingInfo {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = totalItems
            };
            Events = events.ToList().ConvertAll(x => new EventIndexViewModelEventItem(x));
            Users = users;
            EventTypes = eventTypes;
            SelectedUserId = selectedUserId;
            SelectedEventTypeId = selectedEventTypeId;
            CurrentSort = sortOrder;
            CurrentFilter = searchString;
            ApplySort(sortOrder);
            //ApplyFilter(searchString);
        }



        public string UserIdSort { get; private set; }
        public string EventTypeSort { get; private set; }
        public string StartDateSort { get; private set; }
        public string EventSeriesSort { get; private set; }
        public int SelectedUserId { get; private set; }
        public int SelectedEventTypeId { get; private set; }
        
        public IEnumerable<EventIndexViewModelEventItem> Events { get; private set;}

        public SelectList Users { get; private set;}
        public SelectList EventTypes { get; private set;}

        private void ApplySort(string sortOrder)
        {
            switch (sortOrder)
            {
                case "StartDate":
                    Events = Events.OrderBy(x => x.StartDate).ToList();
                    break;
                case "startDate_desc":
                    Events = Events.OrderByDescending(x => x.StartDate).ToList();
                    break;
                case "UserId":
                    Events = Events.OrderBy(x => x.CreatedByUserId).ToList();
                    break;
                case "userId_desc":
                    Events = Events.OrderByDescending(x => x.CreatedByUserId).ToList();
                    break;
                case "EventType":
                    Events = Events.OrderBy(x => x.EventTypeName).ToList();
                    break;
                case "eventType_desc":
                    Events = Events.OrderByDescending(x => x.EventTypeName).ToList(); 
                    break;
                case "EventSeries":
                    Events = Events.OrderBy(x => x.EventSeriesId).ToList();
                    break;
                case "eventSeries_desc":
                    Events = Events.OrderByDescending(x => x.EventSeriesId).ToList();
                    break;
                default:
                    Events = Events.OrderByDescending(x => x.EventId).ToList(); 
                    break;
            }
            StartDateSort = sortOrder == "StartDate" ? "startDate_desc" : "StartDate";
            UserIdSort = sortOrder == "UserId" ? "userId_desc" : "UserId";
            EventTypeSort = sortOrder == "EventType" ? "eventType_desc" : "EventType";
            EventSeriesSort = sortOrder == "EventSeries" ? "eventSeries_desc" : "EventSeries";
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
                Events = Events
                    .Where(x => x.EventTitle.ToLower().Contains(lowerString)
                        || x.EventSeriesTitle.ToLower().Contains(lowerString)
                        || x.CreatedByUserDisplayName.ToLower().Contains(lowerString)
                        || x.CreatedByUserEmail.ToLower().Contains(lowerString))
                    .ToList();
            }
        }
    }

    public class EventIndexViewModelEventItem
    {
        public int EventId { get; private set; }
        public string StartDate { get; private set; }
        public string EndDate { get; private set; }
        public string EventTitle {get; private set; }
        public string EventSeriesTitle { get; private set; }
        public int EventSeriesId { get; private set; }
        public string CreatedByUserDisplayName { get; private set; }
        public string CreatedByUserEmail { get; private set; }
        public string EventTypeName { get; private set; }
        public int CreatedByUserId { get; private set; }
        public string Status { get; private set;}
        public EventIndexViewModelEventItem(EventDto e)
        {
            EventId = e.EventId;            
            StartDate = e.StartDate;
            EndDate = e.EndDate;
            EventTitle = e.EventTitle;
            EventSeriesTitle = e.EventSeriesTitle;
            CreatedByUserDisplayName = e.CreatorName;
            CreatedByUserEmail = e.CreatorEmail;
            EventTypeName = e.EventType;
            CreatedByUserId = e.CreatorId;
            EventSeriesId = e.EventSeriesId;
            Status = e.Status;
        }
        public EventIndexViewModelEventItem(Event e)
        {
            EventId = e.Id;
            StartDate = e.StartDate.ToString("MM/dd/yy HH:mm");
            EndDate = e.EndDate.ToString("MM/dd/yy HH:mm");
            EventTitle = e.Title;
            EventSeriesTitle = e?.EventSeries?.Title ?? "-";
            CreatedByUserDisplayName = e?.Owner?.DisplayName ?? "-";
            CreatedByUserEmail = e?.Owner?.Email ?? "-";
            EventTypeName = e.EventTypeName;
            CreatedByUserId = e?.Owner?.Id ?? 0;
            EventSeriesId = e?.EventSeriesId ?? 0;
            if (e?.Registrations == null)
            {
                Status = "Registrations not loaded";
            }
            else
            {
                Status = e.GetEventStatus();
            }
        }
    }
}
