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
using Microsoft.AspNetCore.Mvc.Rendering;
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
            IEnumerable<Registration> registrations = await unitOfWork.Registrations.GetRegistrationsWithUserAndEvent(SelectedUserId, SelectedEventId, SelectedEventTypeId, page, PageSize);
            SelectList usersSelect = unitOfWork.Users.GetUserSelectList();
            SelectList eventTypeSelect = unitOfWork.EventTypes.GetEventTypeSelectList();
            SelectList eventSelectList = unitOfWork.Events.GetEventSelectList();

            RegistrationIndexViewModel vm = new RegistrationIndexViewModel(
                registrations, 
                usersSelect, 
                eventSelectList, 
                eventTypeSelect,
                SelectedUserId,
                SelectedEventId,
                SelectedEventTypeId,                
                sortOrder, 
                searchString, 
                page, 
                PageSize
                );

            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "RegistrationIndex";
            return View(vm);
        }
    }
}