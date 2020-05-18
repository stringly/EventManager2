using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManager.Data;
using EventManager.Data.Core;
using EventManager.Data.Core.Services;
using EventManager.Extensions;
using EventManager.Models.Domain;
using EventManager.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private IEventService _eventService;
        private int PageSize = 25;

        public EventTypeController(IEventService eventService)
        {
            _eventService = eventService;
        }
        public async Task<IActionResult> Index(string sortOrder, string searchString, int page = 1)
        {
            searchString = searchString.ToLowerRemoveSymbols();
            IEnumerable<EventType> eventTypes = await _eventService.EventTypes.GetEventTypesWithEventsAsync(searchString,page, PageSize);
            int totalItems = 0;
            if (!string.IsNullOrEmpty(searchString))
            {
                Expression<Func<EventType,bool>> predicate = (x => (string.IsNullOrEmpty(searchString) || x.EventTypeName.ToLower().Contains(searchString)));
                totalItems = _eventService.EventTypes.CountByExpression(predicate);
            }
            else
            {
                totalItems = _eventService.EventTypes.Count();
            }
            EventTypeIndexViewModel vm = 
                new EventTypeIndexViewModel(
                eventTypes,
                sortOrder,
                searchString,
                totalItems,
                page,
                PageSize
                );
            
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventTypeIndex";
            ViewData["Title"] = "Event Type Index";
            return View(vm);
        }
        [HttpGet]
        public IActionResult Create(string returnUrl)
        {
            EventTypeAddViewModel vm = new EventTypeAddViewModel();
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventTypeCreate";
            ViewData["Title"] = "Create Event Type";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("EventTypeName")] EventTypeAddViewModel form, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ActiveMenu"] = "Admin";
                ViewData["ActiveLink"] = "EventTypeCreate";
                ViewData["Title"] = "Create Event Type: Error";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            else
            {
                if (!_eventService.CreateEventType(form.EventTypeName, out string response))
                {
                    ModelState.AddModelError("", response);
                    ViewData["ActiveMenu"] = "Admin";
                    ViewData["ActiveLink"] = "EventTypeCreate";
                    ViewData["Title"] = "Create Event Type: Error";
                    ViewBag.ReturnUrl = returnUrl;
                    return View(form);
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
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, string returnUrl)
        {
            if(id == null || !EventTypeExists((int) id))
            {
                return NotFound();
            }
            EventType et = await _eventService.EventTypes.GetEventTypeWithEventsAsync((int)id);
            EventTypeDetailsViewModel vm = new EventTypeDetailsViewModel(et);
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventTypeEdit";
            ViewData["Title"] = "Edit Event Type";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, [Bind("EventTypeId, EventTypeName")] EventTypeDetailsViewModel form, string returnUrl)
        {
            if(id != form.EventTypeId)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                form.InitializeEventsCollection(await _eventService.Events.GetEventsWithRegistrationsForEventTypeIdAsync((int)id));
                ViewData["ActiveMenu"] = "Admin";
                ViewData["ActiveLink"] = "EventTypeEdit";
                ViewData["Title"] = "Edit Event Type: Error";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            else
            {
                if(!_eventService.UpdateEventType(form.EventTypeId, form.EventTypeName, out string response))
                {
                    ModelState.AddModelError("", response);
                    form.InitializeEventsCollection(await _eventService.Events.GetEventsWithRegistrationsForEventTypeIdAsync((int)id));
                    ViewData["ActiveMenu"] = "Admin";
                    ViewData["ActiveLink"] = "EventTypeEdit";
                    ViewData["Title"] = "Edit Event Type: Error";
                    ViewBag.ReturnUrl = returnUrl;
                    return View(form);
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
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id, string returnUrl)
        {
            if (id == null || !EventTypeExists((int)id))
            {
                return NotFound();
            }
            EventType et = await _eventService.EventTypes.GetEventTypeWithEventsAsync((int)id);
            EventTypeDetailsViewModel vm = new EventTypeDetailsViewModel(et);
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventTypeDetails";
            ViewData["Title"] = "Event Type Details";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id, string returnUrl)
        {
            if (id == null || !EventTypeExists((int)id))
            {
                return NotFound();
            }
            EventType et = await _eventService.EventTypes.GetEventTypeWithEventsAsync((int)id);
            EventTypeDetailsViewModel vm = new EventTypeDetailsViewModel(et);
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventTypeDelete";
            ViewData["Title"] = "Delete Event Type";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id, int Id, string returnUrl)
        {
            if(id == null || id != Id || !EventTypeExists(Id))
            {
                return NotFound();
            }
            if(!_eventService.DeleteEventType(Id, out string response))
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
        private bool EventTypeExists(int id)
        {
            return _eventService.EventTypes.Exists(id);
        }
    }
}