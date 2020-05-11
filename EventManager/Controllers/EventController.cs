using EventManager.Data.Core;
using EventManager.Models.Domain;
using EventManager.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Controllers
{
    /// <summary>
    /// Controller that contains actions for the Event Page Views
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class EventController : Controller
    {
        private IUnitOfWork unitOfWork;
        private int PageSize = 25;

        public EventController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, int SelectedEventTypeId = 0, int SelectedUserId = 0, int page = 1)
        {
            EventIndexViewModel vm = new EventIndexViewModel(PageSize);
            vm.CurrentSort = sortOrder;
            vm.CurrentFilter = searchString;
            vm.CreatedDateSort = String.IsNullOrEmpty(sortOrder) ? "createdDate_desc" : "";
            vm.StartDateSort = sortOrder == "StartDate" ? "startDate_desc" : "StartDate";
            vm.UserIdSort = sortOrder == "UserId" ? "userId_desc" : "UserId";
            vm.EventTypeSort = sortOrder == "EventType" ? "eventType_desc" : "EventType";
            vm.EventSeriesSort = sortOrder == "EventSeries" ? "eventSeries_desc" : "EventSeries";
            vm.SelectedEventTypeId = SelectedEventTypeId;
            vm.SelectedUserId = SelectedUserId;
            IEnumerable<Event> events = await unitOfWork.Events.GetEventsWithCreatorEventTypeAndSeriesAsnyc(SelectedEventTypeId, SelectedUserId);
            //List<Event> events = new List<Event>();

            //if (SelectedUserId == 0 && SelectedEventTypeId == 0)
            //{
            //    // no user/type filters selected
            //    events = await _context.Events
            //        .Include(x => x.Creator)
            //            .ThenInclude(x => x.Rank)
            //        .Include(x => x.EventType)
            //        .Include(x => x.EventSeries)
            //        .ToListAsync();
            //}
            //else if (SelectedUserId == 0 && SelectedEventTypeId != 0)
            //{
            //    // event type only
            //    events = await _context.Events
            //        .Where(x => x.EventTypeId == SelectedEventTypeId)
            //        .Include(x => x.Creator)
            //            .ThenInclude(x => x.Rank)
            //        .Include(x => x.EventType)
            //        .Include(x => x.EventSeries)
            //        .ToListAsync();
                
            //}
            //else if (SelectedUserId != 0 && SelectedEventTypeId == 0)
            //{
            //    // user only
            //    events = await _context.Events
            //        .Where(x => x.CreatorId == SelectedUserId)
            //        .Include(x => x.Creator)
            //            .ThenInclude(x => x.Rank)
            //        .Include(x => x.EventType)
            //        .Include(x => x.EventSeries)
            //        .ToListAsync();
                
            //}
            //else if(SelectedUserId != 0 && SelectedEventTypeId != 0)
            //{
            //    // user and event type
            //    // event type only
            //    events = await _context.Events
            //        .Where(x => x.EventTypeId == SelectedEventTypeId && x.CreatorId == SelectedUserId)
            //        .Include(x => x.Creator)
            //            .ThenInclude(x => x.Rank)
            //        .Include(x => x.EventType)
            //        .Include(x => x.EventSeries)
            //        .ToListAsync();
            //}
            switch (sortOrder)
            {
                case "createdDate_desc":
                    events = events.OrderBy(x => x.CreatedDate).ToList();
                    break;
                case "StartDate":
                    events = events.OrderBy(x => x.StartDate).ToList();
                    break;
                case "startDate_desc":
                    events = events.OrderByDescending(x => x.StartDate).ToList();
                    break;
                case "UserId":
                    events = events.OrderBy(x => x.CreatorId).ToList();
                    break;
                case "userId_desc":
                    events = events.OrderByDescending(x => x.CreatorId).ToList();
                    break;
                case "EventType":
                    events = events.OrderBy(x => x.EventTypeId).ToList();
                    break;
                case "eventType_desc":
                    events = events.OrderByDescending(x => x.EventTypeId).ToList();
                    break;
                case "EventSeries":
                    events = events.OrderBy(x => x.EventSeriesId).ToList();
                    break;
                case "eventSeries_desc":
                    events = events.OrderByDescending(x => x.EventSeriesId).ToList();
                    break;
                default: 
                    events = events.OrderByDescending(x => x.CreatedDate).ToList();
                    break;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                  || char.IsWhiteSpace(c)
                                  || c == '-')));
                string lowerString = new string(arr);
                lowerString = lowerString.ToLower();
                events = events
                    .Where(x => x.Title.ToLower().Contains(lowerString)
                        || x.EventSeries.Title.ToLower().Contains(lowerString)
                        || x.Creator.DisplayName.ToLower().Contains(lowerString)
                        || x.Creator.Email.ToLower().Contains(lowerString))
                    .ToList();
            }
            vm.InitializeEventList(events.ToList(), page);
            vm.PagingInfo.ItemsPerPage = PageSize;
            vm.PagingInfo.CurrentPage = page;
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventIndex";
            return View(vm);
        }
        

    }
}