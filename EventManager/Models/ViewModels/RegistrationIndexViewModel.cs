using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.IIS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class RegistrationIndexViewModel : IndexViewModel
    {
        private RegistrationIndexViewModel() { }
        public RegistrationIndexViewModel(
        IEnumerable<Registration> registrations,
        SelectList users, SelectList events,
        SelectList eventTypes,
        int selectedUserId,
        int selectedEventId,
        int selectedEventTypeId,
        string sortOrder,
        string searchString,
        int page = 1,
        int pageSize = 25)
        {
            PagingInfo = new PagingInfo
            {
                ItemsPerPage = pageSize,
                CurrentPage = page,
                TotalItems = registrations.Count(),
            };
            Registrations = registrations
                .ToList()
                .ConvertAll(x => new RegistrationIndexViewModelRegistrationItem(x));
            Users = users;
            Events = events;
            EventTypes = eventTypes;
            SelectedUserId = selectedUserId;
            SelectedEventId = selectedEventId;
            SelectedEventTypeId = selectedEventId;
            CurrentSort = sortOrder;
            CurrentFilter = searchString;
            ApplySort(sortOrder);
            ApplyFilter(searchString);

        }
        public string UserIdSort { get; private set; }
        public string EventIdSort { get; private set; }
        public string EventTypeSort { get; private set; }
        public string RegistrationDateSort { get; private set; }
        public int SelectedUserId { get; private set; }
        public int SelectedEventId { get; private set; }
        public int SelectedEventTypeId { get; private set; }

        public IEnumerable<RegistrationIndexViewModelRegistrationItem> Registrations { get; private set;}

        public SelectList Users { get; private set; }
        public SelectList Events { get; private set; }
        public SelectList EventTypes { get; private set;}

        private void ApplySort(string sortOrder)
        {            
            switch (sortOrder)
            {
                case "registrationDate_desc":
                    Registrations = Registrations.OrderBy(x => x.RegistrationDate).ToList();
                    break;
                case "UserId":
                    Registrations = Registrations.OrderBy(x => x.UserId).ToList();
                    break;
                case "userId_desc":
                    Registrations = Registrations.OrderByDescending(x => x.UserId).ToList();
                    break;
                case "EventId":
                    Registrations = Registrations.OrderBy(x => x.EventId).ToList();
                    break;
                case "eventId_desc":
                    Registrations = Registrations.OrderByDescending(x => x.EventId).ToList();
                    break;
                case "EventTypeId":
                    Registrations = Registrations.OrderBy(x => x.EventTypeName).ToList();
                    break;
                case "eventTypeId_desc":
                    Registrations = Registrations.OrderByDescending(x => x.EventTypeName).ToList();
                    break;
                default:
                    Registrations = Registrations.OrderByDescending(x => x.RegistrationDate).ToList();
                    break;
            }
            RegistrationDateSort = String.IsNullOrEmpty(sortOrder) ? "registrationDate_desc" : "";
            UserIdSort = sortOrder == "UserId" ? "userId_desc" : "UserId";
            EventIdSort = sortOrder == "EventId" ? "eventId_desc" : "EventId";
            EventTypeSort = sortOrder == "EventTypeId" ? "eventTypeId_desc" : "EventTypeId";
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
                Registrations = Registrations
                    .Where(x => x.UserName.ToLower().Contains(lowerString)
                        || x.EventTitle.ToLower().Contains(lowerString))
                    .ToList();
            }
        }
    }
    public class RegistrationIndexViewModelRegistrationItem
    {
        public int RegistrationId { get; private set; }
        public string UserName { get; private set; }
        public int? UserId { get; private set; }
        public string EventTitle { get; private set; }
        public int EventId { get; private set; }
        public string EventTypeName { get; private set; }
        public string RegistrationDate { get; private set; }
        public string EventDate { get; private set; }

        public RegistrationIndexViewModelRegistrationItem(Registration r)
        {
            if (r == null)
            {
                throw new ArgumentNullException("Registration parameter must not be null", nameof(r));
            }
            else if (r.User == null)
            {
                throw new ArgumentNullException("User object of Registration must be loaded to invoke constructor", nameof(r.User));
            }
            else if (r.Event == null)
            {
                throw new ArgumentNullException("Event object of Registration must be loaded to invoke constructor", nameof(r.Event));
            }
            else if (r.Event.EventType == null)
            {
                throw new ArgumentNullException("Event Type object of Registration.Event must be loaded to invoke constructor", nameof(r.Event.EventType));
            }
            else
            {
                RegistrationId = r.Id;
                UserName = r.UserDisplayName;
                UserId = r.UserId;
                EventTitle = r.Event.Title;
                EventId = r.Event.Id;
                EventTypeName = r.Event.EventType.EventTypeName;
                RegistrationDate = r.TimeStamp.ToString("MM/dd/yy HH:mm");
                EventDate = r.Event.StartDate.ToString("HH/dd/yy HH:mm");
            }

        }
    }
}
