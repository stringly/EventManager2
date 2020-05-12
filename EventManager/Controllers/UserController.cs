using EventManager.Data.Core;
using EventManager.Models.Domain;
using EventManager.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private IUnitOfWork unitOfWork;
        private int PageSize = 25;

        public UserController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(string sortOrder, string searchString, int SelectedRankId = 0, int page = 1)
        {
            IEnumerable<User> users = await unitOfWork.Users.GetUsersWithRankAsync(SelectedRankId, page, PageSize);
            SelectList ranks = unitOfWork.Ranks.GetRankSelectList();
            UserIndexViewModel vm = new UserIndexViewModel(
                users,
                ranks,
                SelectedRankId,
                sortOrder, 
                searchString,
                page,
                PageSize);
                 
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "UserIndex";
            return View(vm);
        }
    }
}