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
            IEnumerable<EventType> eventTypes = await unitOfWork.EventTypes.GetEventTypesWithEventsAsync(0,page, PageSize);
            EventTypeIndexViewModel vm = 
                new EventTypeIndexViewModel(
                eventTypes,
                sortOrder,
                searchString,
                page,
                PageSize
                );
            
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventTypeIndex";
            return View(vm);
        }
    }
}