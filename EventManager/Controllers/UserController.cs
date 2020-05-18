using EventManager.Data.Core;
using EventManager.Data.Core.Services;
using EventManager.Extensions;
using EventManager.Models.Domain;
using EventManager.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventManager.Controllers
{
    /// <summary>
    /// Controller that contains actions for the User Page Views
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly int PageSize = 25;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index(string sortOrder, string searchString, int SelectedRankId = 0, int page = 1)
        {
            searchString = searchString.ToLowerRemoveSymbols();
            IEnumerable<User> users = await _userService.Users.GetUsersWithRankAsync(searchString, SelectedRankId, page, PageSize);
            int totalItems = 0;
            if(string.IsNullOrEmpty(searchString) && SelectedRankId == 0)
            {
                totalItems = _userService.Users.Count();
            }
            else
            {
                Expression<Func<User, bool>> predicate = (x => (string.IsNullOrEmpty(searchString) || (x.LDAPName.ToLower().Contains(searchString) || x.IdNumber.ToLower().Contains(searchString) || x.Email.ToLower().Contains(searchString))) && (SelectedRankId == 0 || x.RankId == SelectedRankId));
                totalItems = _userService.Users.CountByExpression(predicate);
            }
            SelectList ranks = _userService.Ranks.GetRankSelectList();
            UserIndexViewModel vm = new UserIndexViewModel(
                users,
                ranks,
                SelectedRankId,
                sortOrder, 
                searchString,
                totalItems,
                page,
                PageSize);
                 
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "UserIndex";
            ViewData["Title"] = "User Index";
            return View(vm);
        }
        [HttpGet]
        public IActionResult Create(string returnUrl)
        {
            UserAddViewModel vm = new UserAddViewModel(_userService.Ranks.GetRankSelectList());
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "UserCreate";
            ViewData["Title"] = "Create User";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpPost]
        public IActionResult Create([Bind("LDAPName,BlueDeckId,FirstName,LastName,IdNumber,Email,ContactNumber,RankId")] UserAddViewModel form, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                form.InitializeRanksList(_userService.Ranks.GetRankSelectList());
                ViewData["ActiveMenu"] = "Admin";
                ViewData["ActiveLink"] = "UserCreate";
                ViewData["Title"] = "Create User: Error";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            else
            {
                if(!_userService.CreateUser(form.LDAPName, (uint)form.BlueDeckId, form.FirstName, form.LastName, form.IdNumber, form.Email, form.ContactNumber, form.RankId, out string response))
                {
                    ModelState.AddModelError("", response);
                    form.InitializeRanksList(_userService.Ranks.GetRankSelectList());
                    ViewData["ActiveMenu"] = "Admin";
                    ViewData["ActiveLink"] = "UserCreate";
                    ViewData["Title"] = "Create User: Error";
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
            if (id == null || !UserExists((int)id))
            {
                return NotFound();
            }
            User toEdit = await _userService.Users.GetUserWithOwnedEventsAndRegistrationsAsync((int)id);
            UserEditViewModel vm = new UserEditViewModel(toEdit, _userService.Ranks.GetRankSelectList());
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "UserEdit";
            ViewData["Title"] = "Edit User";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, [Bind("UserId,LDAPName,BlueDeckId,FirstName,LastName,IdNumber,Email,ContactNumber,RankId")] UserEditViewModel form, string returnUrl)
        {
            if(id != form.UserId)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                form.InitializeRanksList(await _userService.Ranks.GetRankSelectListAsync());
                form.InitializeRegistrationsList(await _userService.Registrations.GetRegistrationsForUserIdByUserIdAsync(form.UserId));
                form.InitializeOwnedEventsList(await _userService.Events.GetEventsOwnedByUserByUserIdAsync(form.UserId));
                ViewData["ActiveMenu"] = "Admin";
                ViewData["ActiveLink"] = "UserCreate";
                ViewData["Title"] = "Edit User: Error";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            else
            {
                if (!_userService.UpdateUser(form.UserId, form.LDAPName, (uint)form.BlueDeckId, form.FirstName, form.LastName, form.IdNumber, form.Email, form.ContactNumber, form.RankId, out string response))
                {
                    ModelState.AddModelError("", response);
                    form.InitializeRanksList(await _userService.Ranks.GetRankSelectListAsync());
                    form.InitializeRegistrationsList(await _userService.Registrations.GetRegistrationsForUserIdByUserIdAsync(form.UserId));
                    form.InitializeOwnedEventsList(await _userService.Events.GetEventsOwnedByUserByUserIdAsync(form.UserId));
                    ViewData["ActiveMenu"] = "Admin";
                    ViewData["ActiveLink"] = "UserCreate";
                    ViewData["Title"] = "Edit User: Error";
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
            if (id == null || !UserExists((int)id))
            {
                return NotFound();
            }
            User toEdit = await _userService.Users.GetUserWithOwnedEventsAndRegistrationsAsync((int)id);
            UserEditViewModel vm = new UserEditViewModel(toEdit, _userService.Ranks.GetRankSelectList());
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "UserEdit";
            ViewData["Title"] = "Edit User";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id, string returnUrl)
        {
            if (id == null || !UserExists((int)id))
            {
                return NotFound();
            }
            User toEdit = await _userService.Users.GetUserWithOwnedEventsAndRegistrationsAsync((int)id);
            UserEditViewModel vm = new UserEditViewModel(toEdit, _userService.Ranks.GetRankSelectList());
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "UserEdit";
            ViewData["Title"] = "Edit User";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int? id, int Id, string returnUrl)
        {
            if (id == null || id != Id || !UserExists(Id))
            {
                return NotFound();
            }
            if (!_userService.RemoveUser(Id, out string response))
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
        private bool UserExists(int id)
        {
            return _userService.Users.Exists(id);
        }
    }
}