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
            EventSeriesIndexViewModel vm = new EventSeriesIndexViewModel(PageSize);
            vm.CurrentSort = sortOrder;
            vm.CurrentFilter = searchString;
            vm.EventSeriesIdSort = String.IsNullOrEmpty(sortOrder) ? "eventSeriesId_desc" : "";
            vm.TitleSort = sortOrder == "EventTitle" ? "eventTitle_desc" : "EventTitle";

            IEnumerable<EventSeries> eventSeries = await unitOfWork.EventSeries.GetAllAsync();
            
            switch (sortOrder)
            {
                case "eventSeriesId_desc":
                    eventSeries = eventSeries.OrderBy(x => x.Id).ToList();
                    break;
                case "EventTitle":
                    eventSeries = eventSeries.OrderBy(x => x.Title).ToList();
                    break;
                case "eventTitle_desc":
                    eventSeries = eventSeries.OrderByDescending(x => x.Title).ToList();
                    break;                
                default:
                    eventSeries = eventSeries.OrderByDescending(x => x.Id).ToList();
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
                eventSeries = eventSeries
                    .Where(x => x.Title.ToLower().Contains(lowerString)
                        || x.Description.ToLower().Contains(lowerString))
                    .ToList();
            }
            vm.IntitializeEventSeriesList(eventSeries.ToList(), page);
            vm.PagingInfo.ItemsPerPage = PageSize;
            vm.PagingInfo.CurrentPage = page;
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "EventSeriesIndex";
            return View(vm);
        }
    }
}