using EventManager.Data.Core;
using EventManager.Data.Core.Services;
using EventManager.Extensions;
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
using System.Linq.Expressions;
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
        private IEventService _eventService;
        private int PageSize = 25;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, int SelectedEventTypeId = 0, int SelectedEventSeriesId = 0, int SelectedUserId = 0, int page = 1)
        {
            searchString = searchString.ToLowerRemoveSymbols();
            var events = await _eventService.GetEvents(searchString, SelectedEventTypeId, SelectedUserId, 0, page, PageSize);
            int totalItems = 0;
            if (!string.IsNullOrEmpty(searchString) || SelectedEventTypeId != 0 || SelectedEventSeriesId != 0 || SelectedUserId != 0)
            {
                Expression<Func<Event, bool>> predicate = (x => (string.IsNullOrEmpty(searchString) || x.Title.Contains(searchString)) && (SelectedEventTypeId == 0 || x.EventTypeId == SelectedEventTypeId) && (SelectedUserId == 0 || x.OwnerId == SelectedUserId) && (SelectedEventSeriesId == 0 || x.EventSeriesId == SelectedEventSeriesId));
                totalItems = _eventService.Events.CountByExpression(predicate);
            }
            else
            {
                totalItems = _eventService.Events.Count();
            }
            
            
            
            SelectList userSelect = _eventService.Users.GetUserSelectList();
            SelectList eventTypeSelect = _eventService.EventTypes.GetEventTypeSelectList();

            EventIndexViewModel vm = new EventIndexViewModel(
                events,
                userSelect,
                eventTypeSelect,
                SelectedUserId,
                SelectedEventTypeId, 
                sortOrder,
                searchString,
                totalItems,
                page,
                PageSize);
            
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventIndex";
            ViewData["Title"] = "Event Index";
            return View(vm);
        }

        public async Task<IActionResult> Create(string returnUrl)
        {
            EventAddViewModel vm = new EventAddViewModel();
            vm.EventTypes = await _eventService.EventTypes.GetEventTypeSelectListAsync();
            vm.EventSerieses = await _eventService.EventSeries.GetEventSeriesSelectListAsync();
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
            "Zip," +
            "Modules"
            )] EventAddViewModel form, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                form.EventTypes = await _eventService.EventTypes.GetEventTypeSelectListAsync();
                form.EventSerieses = await _eventService.EventSeries.GetEventSeriesSelectListAsync();
                form.States = new StaticDataCollection().States;
                ViewBag.ReturnUrl = returnUrl;
                ViewData["Title"] = "Create Event: Error";
                ViewData["ActiveMenu"] = "Event";
                ViewData["ActiveLink"] = "CreateEvent";
                return View(form);
            }
            else
            {
                if (!_eventService.CreateEvent(
                    out string response,
                    form.EventTypeId,
                    0,
                    form.Title,
                    form.Description,
                    form.StartDate,
                    form.EndDate,
                    form.RegistrationOpenDate,
                    form.RegistrationClosedDate,
                    form.AddressLine1,
                    form.AddressLine2,
                    form.City,
                    form.State,
                    form.Zip,
                    form?.EventSeriesId ?? 0,
                    form.MaxRegistrationCount,
                    form?.MinRegistrationCount ?? 0,
                    form.AllowStandby,
                    form?.MaxStandbyRegistrationCount ?? 0,
                    form.FundCenter
                    ))
                {
                    form.EventTypes = await _eventService.EventTypes.GetEventTypeSelectListAsync();
                    form.EventSerieses = await _eventService.EventSeries.GetEventSeriesSelectListAsync();
                    form.States = new StaticDataCollection().States;
                    ModelState.AddModelError("", response);
                    ViewBag.ReturnUrl = returnUrl;
                    ViewData["Title"] = "Create Event: Error";
                    ViewData["ActiveMenu"] = "Event";
                    ViewData["ActiveLink"] = "CreateEvent";
                    return View(form);
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
        [HttpGet]        
        public async Task<IActionResult> Edit(int? id, string returnUrl){
            if (id == null)
            {
                return NotFound();
            }
            else if (!EventExists((int)id))
            {
                return NotFound();
            }
            Event e = await _eventService.Events.GetEventWithCreatorEventTypeAndSeriesAsync((int)id);
            EventEditViewModel vm = new EventEditViewModel(e);
            vm.EventTypes = _eventService.EventTypes.GetEventTypeSelectList();
            vm.EventSerieses = _eventService.EventSeries.GetEventSeriesSelectList();
            vm.States = new StaticDataCollection().States;
            ViewBag.ReturnUrl = returnUrl;
            ViewData["Title"] = "Edit Event";
            ViewData["ActiveMenu"] = "Event";
            ViewData["ActiveLink"] = "EditEvent";
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind(
            "Id," +
            "EventTypeId," +
            "EventSeriesId," +
            "Title," +
            "Description," +
            "FundCenter," +
            "StartDate," +
            "EndDate," +
            "RegistrationOpenDate," +
            "RegistrationClosedDate," +
            "MinRegistrationCount," +
            "MaxRegistrationCount," +
            "AllowStandby," +
            "MaxStandbyRegistrationCount," +
            "AddressLine1," +
            "AddressLine2," +
            "City," +
            "State," +
            "Zip"
            )] EventEditViewModel form, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }
            else if ((int)id != form.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                form.EventTypes = await _eventService.EventTypes.GetEventTypeSelectListAsync();
                form.EventSerieses = await _eventService.EventSeries.GetEventSeriesSelectListAsync();
                form.States = new StaticDataCollection().States;
                ViewBag.ReturnUrl = returnUrl;
                ViewData["Title"] = "Create Event: Error";
                ViewData["ActiveMenu"] = "Event";
                ViewData["ActiveLink"] = "CreateEvent";
                return View(form);
            }
            else
            {
                if (!_eventService.UpdateEvent(
                    out string response,
                    form.Id,
                    form.EventTypeId,
                    0,
                    form.Title,
                    form.Description,
                    form.StartDate,
                    form.EndDate,
                    form.RegistrationOpenDate,
                    form.RegistrationClosedDate,
                    form.AddressLine1,
                    form.AddressLine2,
                    form.City,
                    form.State,
                    form.Zip,
                    form?.EventSeriesId ?? 0,
                    form.MaxRegistrationCount,
                    form?.MinRegistrationCount ?? 0,
                    form.AllowStandby,
                    form?.MaxStandbyRegistrationCount ?? 0,
                    form.FundCenter))
                {
                    form.EventTypes = await _eventService.EventTypes.GetEventTypeSelectListAsync();
                    form.EventSerieses = await _eventService.EventSeries.GetEventSeriesSelectListAsync();
                    form.States = new StaticDataCollection().States;
                    ModelState.AddModelError("", response);
                    ViewBag.ReturnUrl = returnUrl;
                    ViewData["Title"] = "Edit Event: Error";
                    ViewData["ActiveMenu"] = "Event";
                    ViewData["ActiveLink"] = "EditEvent";
                    return View(form);
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
        public async Task<IActionResult> Details(int? id, string returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }
            else if (!EventExists((int)id))
            {
                return NotFound();
            }
            Event e = await _eventService.Events.GetEventWithCreatorEventTypeSeriesAndRegistrationsAsync((int)id);
            if (e == null)
            {
                return NotFound();
            }
            EventDetailsViewModel vm = new EventDetailsViewModel(e);
            ViewBag.ReturnUrl = returnUrl;
            ViewData["Title"] = "Event Details";
            ViewData["ActiveMenu"] = "Event";
            ViewData["ActiveLink"] = "EventDetails";
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id, string returnUrl)
        {
            if(id == null)
            {
                return NotFound();
            }
            else if (!EventExists((int)id))
            {
                return NotFound();
            }
            Event e = await _eventService.Events.GetEventWithCreatorEventTypeSeriesAndRegistrationsAsync((int)id);
            if (e == null)
            {
                return NotFound();
            }
            EventDetailsViewModel vm = new EventDetailsViewModel(e);
            ViewBag.ReturnUrl = returnUrl;
            ViewData["Title"] = "Delete Event?";
            ViewData["ActiveMenu"] = "Event";
            ViewData["ActiveLink"] = "DeleteEvent";
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id, int Id, string returnUrl)
        {
            if (id == null || id != Id || EventExists((int) Id) == false)
            {
                return NotFound();
            }
            if (!_eventService.DeleteEvent((int)Id, out string response))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
            else
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEventModule([Bind("Modules")] EventAddViewModel form)
        {
            if (form.Modules.Count() > 0)
            {
                if (ModelState.GetFieldValidationState("Modules") != Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid)
                {
                    return PartialView("EventModules", form);
                }
            }            
            form.Modules.Add(new EventModuleViewModel());
            return PartialView("EventModules", form);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveEventModule([Bind("Modules")] EventAddViewModel form)
        {
            ModelState.Clear();
            if (form.Modules.Count() > 0)
            {
                form.Modules = form.Modules.Where(x => x.Deleted == false).ToList();
            }
            return PartialView("EventModules", form);
        }
        private bool EventExists(int id)
        {
            return _eventService.Events.Exists(id);
        }

    }
}