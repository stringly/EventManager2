using EventManager.Data.Core;
using EventManager.Data.Core.Services;
using EventManager.Models.Domain;
using EventManager.Models.Enums;
using EventManager.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
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
        private IEventService eventService;
        private int PageSize = 25;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, int SelectedEventTypeId = 0, int SelectedUserId = 0, int page = 1)
        {
            IEnumerable<Event> events = await eventService.Events.GetEventsWithCreatorEventTypeAndSeriesAsnyc(SelectedEventTypeId, SelectedUserId, 0, page, PageSize);
            SelectList userSelect = eventService.Users.GetUserSelectList();
            SelectList eventTypeSelect = eventService.EventTypes.GetEventTypeSelectList();

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

        public async Task<IActionResult> Create(string returnUrl)
        {
            EventAddViewModel vm = new EventAddViewModel();
            vm.EventTypes = eventService.EventTypes.GetEventTypeSelectList();
            vm.EventSerieses = eventService.EventSeries.GetEventSeriesSelectList();
            vm.States = new StaticDataCollection().States;
            ViewBag.ReturnUrl = returnUrl;
            ViewData["Title"] = "Create Event";
            ViewData["ActiveMenu"] = "Event";
            ViewData["ActiveLink"] = "CreateEvent";
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(
            "EventTypeId," +
            "EventSeriesId," +
            "Title," +
            "Description,"+
            "FundCenter," +
            "StartDate," +
            "EndDate," +
            "RegistrationOpenDate," +
            "RegistrationClosedDate," +
            "MinRegistrationCount," +
            "MaxRegistrationCount," +
            "AllowStandby," +
            "MaxStandbyRegistrationCount,"+
            "AddressLine1," +
            "AddressLine2," +
            "City," +
            "State," +
            "Zip"
            )] EventAddViewModel form, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                form.EventTypes = eventService.EventTypes.GetEventTypeSelectList();
                form.EventSerieses = eventService.EventSeries.GetEventSeriesSelectList();
                form.States = new StaticDataCollection().States;
                ViewBag.ReturnUrl = returnUrl;
                ViewData["Title"] = "Create Event: Error";
                ViewData["ActiveMenu"] = "Event";
                ViewData["ActiveLink"] = "CreateEvent";
                return View(form);
            }
            else
            {
                if (!eventService.CreateEvent(form, out string response)){
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
                else
                {
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
        }
        

    }
}