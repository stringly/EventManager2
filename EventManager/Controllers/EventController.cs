using EventManager.Data.Core;
using EventManager.Models.Domain;
using EventManager.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            IEnumerable<Event> events = await unitOfWork.Events.GetEventsWithCreatorEventTypeAndSeriesAsnyc(SelectedEventTypeId, SelectedUserId, 0, page, PageSize);
            SelectList userSelect = unitOfWork.Users.GetUserSelectList();
            SelectList eventTypeSelect = unitOfWork.EventTypes.GetEventTypeSelectList();

            EventIndexViewModel vm = new EventIndexViewModel(
                events,
                userSelect,
                eventTypeSelect,
                SelectedUserId,
                SelectedEventTypeId, 
                sortOrder,
                searchString,
                page,
                PageSize);
            
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventIndex";
            return View(vm);
        }
        

    }
}