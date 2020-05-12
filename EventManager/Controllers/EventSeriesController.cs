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

namespace EventManager.Controllers
{
    /// <summary>
    /// Controller that contains actions for the Event Series Page Views
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class EventSeriesController : Controller
    {
        private IUnitOfWork unitOfWork;
        private int PageSize = 25;

        public EventSeriesController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, int page = 1)
        {
            IEnumerable<EventSeries> eventSeries = await unitOfWork.EventSeries.GetEventSeriesWithEventsAsync(0,page,PageSize);
            EventSeriesIndexViewModel vm = new EventSeriesIndexViewModel(
                    eventSeries,
                    sortOrder,
                    searchString,
                    page,
                    PageSize
                );
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventSeriesIndex";
            return View(vm);
        }
    }
}