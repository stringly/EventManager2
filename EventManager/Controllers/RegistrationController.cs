using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EventManager.Data;
using EventManager.Data.Core;
using EventManager.Data.Core.Services;
using EventManager.Models.Domain;
using EventManager.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IRegistrationService _registrationService;
        private readonly int PageSize = 25;
        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }
        public async Task<IActionResult> Index(string sortOrder, string SelectedStatus, int SelectedEventId = 0, int SelectedUserId = 0, int page = 1)
        {            
            IEnumerable<Registration> registrations = await _registrationService.Registrations.GetRegistrationsWithUserAndEventAsync(SelectedStatus, SelectedUserId, SelectedEventId, page, PageSize);
            int totalItems = 0;
            if(SelectedEventId == 0 && SelectedUserId == 0 && string.IsNullOrEmpty(SelectedStatus))
            {
                totalItems = _registrationService.Registrations.Count();
            }
            else
            {
                Expression<Func<Registration, bool>> predicate = (x => ( string.IsNullOrEmpty(SelectedStatus) ||x.Status.ToString() == SelectedStatus) && (SelectedEventId == 0 || x.EventId == SelectedEventId) && (SelectedUserId == 0 || x.UserId == SelectedUserId));
                totalItems = _registrationService.Registrations.CountByExpression(predicate);
            }
            SelectList usersSelect = _registrationService.Users.GetUserSelectList();
            SelectList eventSelectList = await _registrationService.Events.GetEventSelectListAsync();
            SelectList regStatuses = _registrationService.Registrations.GetRegistrationStatuses();
            RegistrationIndexViewModel vm = new RegistrationIndexViewModel(
                registrations, 
                usersSelect, 
                eventSelectList,
                regStatuses,
                SelectedUserId,
                SelectedEventId,
                SelectedStatus,
                sortOrder, 
                totalItems,
                page, 
                PageSize
                );

            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "RegistrationIndex";
            ViewData["Title"] = "Registration Index";
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Create(string returnUrl)
        {
            RegistrationAddViewModel vm = new RegistrationAddViewModel
            {
                Users = await _registrationService.Users.GetUserSelectListAsync(),
                Events = await _registrationService.Events.GetEventSelectListAsync(),
                RegistrationStatuses = _registrationService.Registrations.GetRegistrationStatuses()
            };
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "RegistrationCreate";
            ViewData["Title"] = "Create Registration";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SelectedEventId,SelectedUserId,SelectedRegistrationStatus")] RegistrationAddViewModel form, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                form.Users = await _registrationService.Users.GetUserSelectListAsync();
                form.Events = await _registrationService.Events.GetEventSelectListAsync();
                form.RegistrationStatuses = _registrationService.Registrations.GetRegistrationStatuses();
                ViewData["ActiveMenu"] = "Admin";
                ViewData["ActiveLink"] = "RegistrationCreate";
                ViewData["Title"] = "Create Registration";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            if(!_registrationService.CreateRegistration(form.SelectedUserId, form.SelectedEventId, out string response))
            {
                ModelState.AddModelError("", response);
                form.Users = await _registrationService.Users.GetUserSelectListAsync();
                form.Events = await _registrationService.Events.GetEventSelectListAsync();
                form.RegistrationStatuses = _registrationService.Registrations.GetRegistrationStatuses();
                ViewData["ActiveMenu"] = "Admin";
                ViewData["ActiveLink"] = "RegistrationCreate";
                ViewData["Title"] = "Create Registration";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            else
            {
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id, string returnUrl)
        {
            if(id == null || !RegistrationExists((int)id))
            {
                return NotFound();
            }
            Registration r = await _registrationService.Registrations.GetRegistrationWithUserAndEventByRegistrationIdAsync((int)id);
            RegistrationDetailsViewModel vm = new RegistrationDetailsViewModel(r);
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "RegistrationDetails";
            ViewData["Title"] = "Registration Details";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id, string returnUrl)
        {
            if (id == null || !RegistrationExists((int)id))
            {
                return NotFound();
            }
            Registration r = await _registrationService.Registrations.GetRegistrationWithUserAndEventByRegistrationIdAsync((int)id);
            RegistrationDetailsViewModel vm = new RegistrationDetailsViewModel(r);
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "RegistrationDelete";
            ViewData["Title"] = "Delete Registration?";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id, int Id, string returnUrl)
        {
            if (id == null || id != Id || !RegistrationExists(Id))
            {
                return NotFound();
            }
            if (!_registrationService.DeleteRegistration(Id, out string response))
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
        private bool RegistrationExists(int id)
        {
            return _registrationService.Registrations.Exists(id);
        }
    }
}