using EventManager.Data.Core;
using EventManager.Models.Domain;
using EventManager.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        private IUnitOfWork unitOfWork;
        private int PageSize = 25;

        public RankController(IUnitOfWork unit)
        {
            unitOfWork = unit;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString, int page = 1)
        {
            IEnumerable<Rank> ranks = await unitOfWork.Ranks.GetRanksWithUsersAsync(page, PageSize);
            RankIndexViewModel vm = new RankIndexViewModel(
                ranks,
                sortOrder,
                searchString,
                page,
                PageSize);            
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "RankIndex";
            return View(vm);
        }
    }
}