using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventManager.Data;
using EventManager.Data.Core;
using EventManager.Models.Domain;
using EventManager.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Controllers
{
    /// <summary>
    /// Controller that contains actions for the Event Type Page Views
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class EventTypeController : Controller
    {
        private IUnitOfWork unitOfWork;
        private int PageSize = 25;

        public EventTypeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(string sortOrder, string searchString, int page = 1)
        {
            EventTypeIndexViewModel vm = new EventTypeIndexViewModel(PageSize);
            vm.CurrentSort = sortOrder;
            vm.CurrentFilter = searchString;
            vm.TypeNameSortOrder = String.IsNullOrEmpty(sortOrder) ? "eventTypeName_desc" : "";
            vm.TypeEventCountSortOrder = sortOrder == "EventCount" ? "eventCount_desc" : "EventCount";

            IEnumerable<EventType> eventTypes = await unitOfWork.EventTypes.GetAllAsync();
            switch (sortOrder)
            {
                case "eventTypeName_desc":
                    eventTypes = eventTypes.OrderByDescending(x => x.EventTypeName).ToList();
                    break;
                case "EventCount":
                    eventTypes = eventTypes.OrderBy(x => x.Events.Count()).ToList();
                    break;
                case "eventCount_desc":
                    eventTypes = eventTypes.OrderByDescending(x => x.Events.Count()).ToList();
                    break;
                default:
                    eventTypes = eventTypes.OrderBy(x => x.EventTypeName).ToList();
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
                eventTypes = eventTypes
                    .Where(x => x.EventTypeName.ToLower().Contains(lowerString))
                    .ToList();
            }
            vm.InitializeEventTypeList(eventTypes.ToList(), page);
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventTypeIndex";
            return View(vm);
        }
    }
}