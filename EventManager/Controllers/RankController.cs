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
            RankIndexViewModel vm = new RankIndexViewModel(PageSize);
            vm.CurrentSort = sortOrder;
            vm.CurrentFilter = searchString;
            vm.RankIdSort = String.IsNullOrEmpty(sortOrder) ? "rankId_desc" : "";
            vm.RankNameSort = sortOrder == "RankName" ? "rankName_desc" : "RankName";
            IEnumerable<Rank> ranks = await unitOfWork.Ranks.GetAllAsync();

            switch (sortOrder)
            {
                case "RankName":
                    ranks = ranks.OrderBy(x => x.FullName).ToList();
                    break;
                case "rankName_desc":
                    ranks = ranks.OrderByDescending(x => x.FullName).ToList();
                    break;
                case "rankId_desc":
                    ranks = ranks.OrderByDescending(x => x.Id).ToList();
                    break;
                default:
                    ranks = ranks.OrderBy(x => x.Id).ToList();
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
                ranks = ranks
                    .Where(x => x.Full.ToLower().Contains(lowerString) || x.Short.ToLower().Contains(lowerString))
                    .ToList();
            }
            vm.InitializeRankList(ranks.ToList(), page);
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "RankIndex";
            return View(vm);
        }
    }
}