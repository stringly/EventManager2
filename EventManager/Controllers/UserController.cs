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
            UserIndexViewModel vm = new UserIndexViewModel(PageSize);
            vm.CurrentSort = sortOrder;
            vm.CurrentFilter = searchString;
            vm.LastNameSort = String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
            vm.BadgeNumberSort = sortOrder == "BadgeNumber" ? "badgeNumber_desc" : "BadgeNumber";
            vm.RankSort = sortOrder == "Rank" ? "rank_desc" : "Rank";
            vm.SelectedRankId = SelectedRankId;

            IEnumerable<User> users = await unitOfWork.Users.GetUsersByRankAsync(SelectedRankId);

            switch (sortOrder)
            {
                case "BadgeNumber":
                    users = users.OrderBy(x => x.IdNumber).ToList();
                    break;
                case "badgeNumber_desc":
                    users = users.OrderByDescending(x => x.IdNumber).ToList();
                    break;
                case "Rank":
                    users = users.OrderBy(x => x.RankId).ToList();
                    break;
                case "rank_desc":
                    users = users.OrderByDescending(x => x.RankId).ToList();
                    break;
                case "lastName_desc":
                    users = users.OrderByDescending(x => x.LastName).ToList();
                    break;
                default:
                    users = users.OrderBy(x => x.LastName).ToList();
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
                users = users
                    .Where(x => x.FirstName.ToLower().Contains(lowerString)
                        || x.LastName.ToLower().Contains(lowerString)
                        || x.Email.ToLower().Contains(lowerString)
                        || x.IdNumber.ToLower().Contains(lowerString))
                    .ToList();
            }
            vm.InitializeUserList(users.ToList(), page);            
            ViewData["ActiveMenu"] = "Admin";
            ViewData["ActiveLink"] = "UserIndex";
            return View(vm);
        }
    }
}