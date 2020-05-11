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
    /// Controller that contains actions for the Registration Page Views
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RegistrationController : Controller
    {
        private IUnitOfWork unitOfWork;
        private int PageSize = 25;
        public RegistrationController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(string sortOrder, string searchString, int SelectedEventId = 0, int SelectedUserId = 0, int SelectedEventTypeId = 0, int page = 1)
        {
            RegistrationIndexViewModel vm = new RegistrationIndexViewModel(PageSize);
            vm.CurrentSort = sortOrder;
            vm.CurrentFilter = searchString;
            vm.RegistrationDateSort = String.IsNullOrEmpty(sortOrder) ? "registrationDate_desc" : "";
            vm.UserIdSort = sortOrder == "UserId" ? "userId_desc" : "UserId";
            vm.EventIdSort = sortOrder == "EventId" ? "eventId_desc" : "EventId";
            vm.EventTypeSort = sortOrder == "EventTypeId" ? "eventTypeId_desc" : "EventTypeId";
            vm.SelectedEventId = SelectedEventId;
            vm.SelectedUserId = SelectedUserId;
            vm.SelectedEventTypeId = SelectedEventTypeId;
            IEnumerable<Registration> registrations = await unitOfWork.Registrations.GetRegistrationsWithUserAndEvent(SelectedUserId, SelectedEventId, SelectedEventTypeId);
            //List<Registration> registrations = new List<Registration>();

            //if(SelectedEventId == 0 && SelectedUserId == 0 && SelectedEventTypeId == 0)
            //{
            //    // no filters
            //    registrations = await _context.Registrations
            //        .Include(x => x.User)
            //            .ThenInclude(x => x.Rank)
            //        .Include(x => x.Event)
            //            .ThenInclude(x => x.EventType)
            //        .ToListAsync();
            //}
            //else if(SelectedEventId != 0 && SelectedUserId == 0 && SelectedEventTypeId == 0)
            //{
            //    // Event ID Only
            //    registrations = await _context.Registrations
            //        .Where(x => x.EventId == SelectedEventId)
            //        .Include(x => x.User)
            //            .ThenInclude(x => x.Rank)
            //        .Include(x => x.Event)
            //            .ThenInclude(x => x.EventType)
            //        .ToListAsync();
                
            //}
            //else if (SelectedEventId != 0 && SelectedUserId != 0 && SelectedEventTypeId == 0)
            //{
            //    // Event ID and User ID
            //    registrations = await _context.Registrations
            //        .Where(x => x.EventId == SelectedEventId && x.UserId == SelectedUserId)
            //        .Include(x => x.User)
            //            .ThenInclude(x => x.Rank)
            //        .Include(x => x.Event)
            //            .ThenInclude(x => x.EventType)
            //        .ToListAsync();
                
            //}
            //else if (SelectedEventId != 0 && SelectedUserId != 0 && SelectedEventTypeId != 0)
            //{
            //    // all 3
            //    registrations = await _context.Registrations
            //        .Where(x => x.EventId == SelectedEventId && x.UserId == SelectedUserId && x.Event.EventTypeId == SelectedEventTypeId)
            //        .Include(x => x.User)
            //            .ThenInclude(x => x.Rank)
            //        .Include(x => x.Event)
            //            .ThenInclude(x => x.EventType)
            //        .ToListAsync();
                
            //}
            //else if (SelectedEventId != 0 && SelectedUserId == 0 && SelectedEventTypeId != 0)
            //{
            //    // Event ID and EventTypeId
            //    registrations = await _context.Registrations
            //        .Where(x => x.EventId == SelectedEventId && x.Event.EventTypeId == SelectedEventTypeId)
            //        .Include(x => x.User)
            //            .ThenInclude(x => x.Rank)
            //        .Include(x => x.Event)
            //            .ThenInclude(x => x.EventType)
            //        .ToListAsync();
            //}
            //else if (SelectedEventId == 0 && SelectedUserId != 0 && SelectedEventTypeId != 0)
            //{
            //    // User ID Only
            //    registrations = await _context.Registrations
            //        .Where(x => x.UserId == SelectedUserId)
            //        .Include(x => x.User)
            //            .ThenInclude(x => x.Rank)
            //        .Include(x => x.Event)
            //            .ThenInclude(x => x.EventType)
            //        .ToListAsync();
            //}
            //else if (SelectedEventId == 0 && SelectedUserId != 0 && SelectedEventTypeId != 0)
            //{
            //    // User ID and EventType ID
            //    registrations = await _context.Registrations
            //        .Where(x => x.UserId == SelectedUserId && x.Event.EventTypeId == SelectedEventTypeId)
            //        .Include(x => x.User)
            //            .ThenInclude(x => x.Rank)
            //        .Include(x => x.Event)
            //            .ThenInclude(x => x.EventType)
            //        .ToListAsync();
            //}
            //else if (SelectedEventId == 0 && SelectedUserId == 0 && SelectedEventTypeId != 0)
            //{
            //    // EventType ID Only
            //    registrations = await _context.Registrations
            //        .Where(x => x.Event.EventTypeId == SelectedEventTypeId)
            //        .Include(x => x.User)
            //            .ThenInclude(x => x.Rank)
            //        .Include(x => x.Event)
            //            .ThenInclude(x => x.EventType)
            //        .ToListAsync();
            //}



            switch (sortOrder)
            {
                case "registrationDate_desc":
                    registrations = registrations.OrderBy(x => x.TimeStamp).ToList();
                    break;
                case "UserId":
                    registrations = registrations.OrderBy(x => x.UserId).ToList();
                    break;
                case "userId_desc":
                    registrations = registrations.OrderByDescending(x => x.UserId).ToList();
                    break;
                case "EventId":
                    registrations = registrations.OrderBy(x => x.EventId).ToList();
                    break;
                case "eventId_desc":
                    registrations = registrations.OrderByDescending(x => x.EventId).ToList();
                    break;
                case "EventTypeId":
                    registrations = registrations.OrderBy(x => x.Event.EventTypeId).ToList();
                    break;
                case "eventTypeId_desc":
                    registrations = registrations.OrderByDescending(x => x.Event.EventTypeId).ToList();
                    break;
                default:
                    registrations = registrations.OrderByDescending(x => x.TimeStamp).ToList();
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
                registrations = registrations
                    .Where(x => x.User.Name.ToLower().Contains(lowerString)
                        || x.User.Email.ToLower().Contains(lowerString)
                        || x.Event.Title.ToLower().Contains(lowerString))
                    .ToList();
            }
            vm.InitializeRegistrationList(registrations.ToList(), page);
            vm.PagingInfo.ItemsPerPage = PageSize;
            vm.PagingInfo.CurrentPage = page;
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "RegistrationIndex";
            return View(vm);
        }
    }
}