using EventManager.Data.Core;
using EventManager.Data.Core.Services;
using EventManager.Extensions;
using EventManager.Models.Domain;
using EventManager.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Controllers
{
    /// <summary>
    /// Controller that contains actions for the Rank Page Views
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RankController : Controller
    {
        private IUserService _userService;
        private int PageSize = 3;

        public RankController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, int page = 1)
        {
            searchString = searchString.ToLowerRemoveSymbols();
            IEnumerable<Rank> ranks = await _userService.Ranks.GetRanksWithUsersAsync(searchString, page, PageSize);
            int totalItems = 0;
            if (string.IsNullOrEmpty(searchString))
            {
                totalItems = _userService.Ranks.Count();
            }
            else
            {
                totalItems = _userService.Ranks.CountByExpression(x => (x.FullName.ToLower().Contains(searchString) || x.ShortName.ToLower().Contains(searchString)));
            }
             
             
            RankIndexViewModel vm = new RankIndexViewModel(
                ranks,
                sortOrder,
                searchString,
                totalItems,
                page,
                PageSize);            
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "RankIndex";
            ViewData["Title"] = "Rank Index";            
            return View(vm);
        }
        [HttpGet]
        public IActionResult Create(string returnUrl)
        {
            RankAddViewModel vm = new RankAddViewModel();
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "RankCreate";
            ViewData["Title"] = "Create Rank";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Short,Long")] RankAddViewModel form, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ActiveMenu"] = "Admin";
                ViewData["ActiveLink"] = "RankCreate";
                ViewData["Title"] = "Create Rank: Error";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            else
            {
                if (!_userService.CreateRank(form.Short, form.Long, out string response))
                {
                    ModelState.AddModelError("", response);
                    ViewData["ActiveMenu"] = "Admin";
                    ViewData["ActiveLink"] = "RankCreate";
                    ViewData["Title"] = "Create Rank: Error";
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
            if(id == null || !RankExists((int)id))
            {
                return NotFound();
            }
            Rank toEdit = await _userService.Ranks.GetRankWithUsersAsync((int)id);
            if(toEdit == null)
            {
                return NotFound();
            }
            RankEditViewModel vm = new RankEditViewModel(toEdit);
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "RankEdit";
            ViewData["Title"] = "Edit Rank";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Short,Long")] RankEditViewModel form, string returnUrl)
        {
            if(id != form.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                form.InitializeUserList(await _userService.Users.GetUsersByRankIdAsync(form.Id));
                ViewData["ActiveMenu"] = "Admin";
                ViewData["ActiveLink"] = "RankEdit";
                ViewData["Title"] = "Edit Rank: Error";
                ViewBag.ReturnUrl = returnUrl;
                return View(form);
            }
            else
            {
                if (!_userService.UpdateRank(form.Id, form.Short, form.Long, out string response))
                {
                    ModelState.AddModelError("", response);
                    form.InitializeUserList(await _userService.Users.GetUsersByRankIdAsync(form.Id));
                    ViewData["ActiveMenu"] = "Admin";
                    ViewData["ActiveLink"] = "RankEdit";
                    ViewData["Title"] = "Edit Rank: Error";
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
            if (id == null || !RankExists((int)id))
            {
                return NotFound();
            }
            Rank toView = await _userService.Ranks.GetRankWithUsersAsync((int)id);
            RankEditViewModel vm = new RankEditViewModel(toView);
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "RankDetails";
            ViewData["Title"] = "Rank Details";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id, string returnUrl)
        {
            if (id == null || !RankExists((int)id))
            {
                return NotFound();
            }
            Rank toView = await _userService.Ranks.GetRankWithUsersAsync((int)id);
            RankEditViewModel vm = new RankEditViewModel(toView);
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "RankDetails";
            ViewData["Title"] = "Rank Details";
            ViewBag.ReturnUrl = returnUrl;
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id, int Id, string returnUrl)
        {
            if (id == null || id != Id || !RankExists(Id))
            {
                return NotFound();
            }
            if (!_userService.DeleteRank(Id, out string response))
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
        private bool RankExists(int id)
        {
            return _userService.Ranks.Exists(id);
        }
    }
}