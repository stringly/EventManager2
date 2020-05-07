using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventManager.Models;
using Microsoft.AspNetCore.Authorization;
using EventManager.Data;
using System.Security.Claims;

namespace EventManager.Controllers
{
    /// <summary>
    /// Controller that contains actions for the Home Page Views
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        public IActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            if(identity.HasClaim(claim => claim.Type == "UserId"))
            {
                var claimMemberId = Convert.ToInt32(identity.Claims.FirstOrDefault(claim => claim.Type == "MemberId").Value.ToString());
                if(claimMemberId == 0) // memberId 0 means the user is not in the user table.
                {
                    return RedirectToAction(nameof(About));
                }
                else
                {
                    return View();
                }
            }
            // condition if no UserId Claim is present
            return RedirectToAction(nameof(About));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
