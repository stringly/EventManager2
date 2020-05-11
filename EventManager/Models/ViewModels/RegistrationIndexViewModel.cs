using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class RegistrationIndexViewModel : IndexViewModel
    {
        public string UserIdSort { get; set; }
        public string EventIdSort { get; set; }
        public string EventTypeSort { get; set; }
        public string RegistrationDateSort { get; set; }
        public int SelectedUserId { get; set; }
        public int SelectedEventId { get; set; }
        public int SelectedEventTypeId { get; set; }

        public List<RegistrationIndexViewModelRegistrationItem> Registrations { get; private set;}

        public List<SelectListItem> Users { get; private set; }
        public List<SelectListItem> Events { get; private set; }
        public List<SelectListItem> EventTypes { get; private set;}
        public RegistrationIndexViewModel(int pageSize = 25)
        {
            PagingInfo = new PagingInfo { ItemsPerPage = 25 };
        }
        public void InitializeRegistrationList(List<Registration> registrations, int page)
        {
            PagingInfo.CurrentPage = page;
            PagingInfo.TotalItems = registrations.Count();            
            Users = registrations
                .Select(x => x.User)
                .Distinct()
                .ToList()
                .ConvertAll(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString()});
            Events = registrations
                .Select(x => x.Event)
                .Distinct()
                .ToList()
                .ConvertAll(x => new SelectListItem { Text = x.Title, Value = x.Id.ToString()});
            EventTypes = registrations
                .Select(x => x.Event.EventType)
                .Distinct()
                .ToList()
                .ConvertAll(x => new SelectListItem { Text = x.EventTypeName, Value = x.Id.ToString()});
            Registrations = registrations
                .Skip((PagingInfo.CurrentPage - 1) * PagingInfo.ItemsPerPage)
                .Take(PagingInfo.ItemsPerPage)
                .ToList()
                .ConvertAll(x => new RegistrationIndexViewModelRegistrationItem(x));

        }        
    }
    public class RegistrationIndexViewModelRegistrationItem
    {
        public int RegistrationId { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string EventTitle { get; set; }
        public int EventId { get; set; }
        public string EventTypeName { get; set; }
        public string RegistrationDate { get; set; }
        public string EventDate { get; set; }

        public RegistrationIndexViewModelRegistrationItem(Registration r)
        {
            RegistrationId = r.Id;
            UserName = r?.UserDisplayName ?? "-";
            UserId = r?.UserId ?? 0;
            EventTitle = r?.Event?.Title ?? "-";
            EventId = r?.Event?.Id ?? 0;
            EventTypeName = r?.Event?.EventType?.EventTypeName ?? "-";
            RegistrationDate = r.TimeStamp.ToString("MM/dd/yy HH:mm");
            EventDate = r?.Event?.StartDate.ToString("HH/dd/yy HH:mm");
        }

    }
}
