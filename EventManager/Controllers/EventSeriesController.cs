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

namespace EventManager.Controllers
{
    /// <summary>
    /// Controller that contains actions for the Event Series Page Views
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class EventSeriesController : Controller
    {
        private IEventService _eventService;
        private int PageSize = 25;

        public EventSeriesController(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, int page = 1)
        {
            searchString = searchString.ToLowerRemoveSymbols();
            IEnumerable<EventSeries> eventSeries = await _eventService.EventSeries.GetEventSeriesWithEventsAsync(searchString,page,PageSize);
            int totalItems = 0;
            if (!string.IsNullOrEmpty(searchString))
            {
                Expression<Func<EventSeries, bool>> predicate = (x => (string.IsNullOrEmpty(searchString) || x.Title.Contains(searchString)));
                totalItems = _eventService.EventSeries.CountByExpression(predicate);
            }
            else
            {
                totalItems = _eventService.EventSeries.Count();
            }
            EventSeriesIndexViewModel vm = new EventSeriesIndexViewModel(
                    eventSeries,
                    sortOrder,
                    searchString,
                    totalItems,
                    page,
                    PageSize
                );
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventSeriesIndex";
            ViewData["Title"] = "Event Series Index";
            return View(vm);
        }
        [HttpGet]
        public IActionResult Create(string returnUrl)
        {
            EventSeriesAddViewModel vm = new EventSeriesAddViewModel();
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventSeriesCreate";
            ViewData["Title"] = "Create Event Series";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpPost]
        public IActionResult Create([Bind("Title, Description")] EventSeriesAddViewModel form, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ActiveMenu"] = "Admin";
                ViewData["ActiveLink"] = "EventSeriesCreate";
                ViewData["Title"] = "Create Event Series";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            else
            {
                if (!_eventService.CreateEventSeries(form.Title, form.Description, out string response))
                {
                    ModelState.AddModelError("", response);
                    ViewData["ActiveMenu"] = "Admin";
                    ViewData["ActiveLink"] = "EventSeriesCreate";
                    ViewData["Title"] = "Create Event Series";
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
            if (id == null || !EventSeriesExists((int)id)){
                return NotFound();
            }
            EventSeries es = await _eventService.EventSeries.GetEventSeriesWithEventsAndRegistrationsAsync((int)id);
            EventSeriesEditViewModel vm = new EventSeriesEditViewModel(es);
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventSeriesEdit";
            ViewData["Title"] = "Edit Event Series";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id, Title, Description")] EventSeriesEditViewModel form, string returnUrl)
        {
            if ((int)id != form.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                form.InitializeEventList(await _eventService.Events.GetEventsWithRegistrationsForEventSeriesIdAsync(form.Id));
                ViewData["ActiveMenu"] = "Admin";
                ViewData["ActiveLink"] = "EventSeriesEdit";
                ViewData["Title"] = "Edit Event Series: Error";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            else
            {
                if (!_eventService.UpdateEventSeries(form.Id, form.Title, form.Description, out string response))
                {
                    form.InitializeEventList(await _eventService.Events.GetEventsWithRegistrationsForEventSeriesIdAsync(form.Id));
                    ModelState.AddModelError("", response);
                    ViewData["ActiveMenu"] = "Admin";
                    ViewData["ActiveLink"] = "EventSeriesEdit";
                    ViewData["Title"] = "Edit Event Series: Error";
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
            if(id == null || !EventSeriesExists((int)id))
            {
                return NotFound();
            }
            EventSeries es = await _eventService.EventSeries.GetEventSeriesWithEventsAndRegistrationsAsync((int)id);
            EventSeriesEditViewModel vm = new EventSeriesEditViewModel(es);
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventSeriesDetails";
            ViewData["Title"] = "Event Series Details";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id, string returnUrl)
        {
            if(id == null || !EventSeriesExists((int)id))
            {
                return NotFound();
            }
            EventSeries es = await _eventService.EventSeries.GetEventSeriesWithEventsAndRegistrationsAsync((int)id);
            EventSeriesEditViewModel vm = new EventSeriesEditViewModel(es);
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "DeleteEventSeries";
            ViewData["Title"] = "Delete Event Series?";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id, int Id, string returnUrl)
        {
            if (id == null || id != Id || !EventSeriesExists((int)id))
            {
                return NotFound();
            }
            if(!_eventService.DeleteEventSeries(Id, out string response))
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
        public bool EventSeriesExists(int id)
        {
            return _eventService.EventSeries.Exists(id);
        }
    }
}