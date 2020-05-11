using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class UserIndexViewModel : IndexViewModel
    {
        public string LastNameSort { get; set; }
        public string BadgeNumberSort { get; set; }
        public string RankSort { get; set; }
        public int SelectedRankId { get; set; }
        public List<UserIndexViewModelUserItem> Users { get; private set;}

        public List<SelectListItem> Ranks { get; private set;}

        public UserIndexViewModel(int pageSize = 25)
        {
            PagingInfo = new PagingInfo { ItemsPerPage = 25 };
        }

        public void InitializeUserList(List<User> users, int page)
        {
            PagingInfo.CurrentPage = page;
            PagingInfo.TotalItems = users.Count();            
            Ranks = users
                .Select(x => x.Rank)
                .Distinct()
                .ToList()
                .ConvertAll(x => new SelectListItem { Text = x.FullName, Value = x.Id.ToString()});
            Users = users
                .Skip((PagingInfo.CurrentPage - 1) * PagingInfo.ItemsPerPage)
                .Take(PagingInfo.ItemsPerPage)
                .ToList()
                .ConvertAll(x => new UserIndexViewModelUserItem(x));
        }
    }

    public class UserIndexViewModelUserItem
    {
        public int UserId { get; set; }
        public string LDAPName { get;set;}
        public string Rank { get;set;}
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string IdNumber { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public uint BlueDeckId { get;set;}
        public UserIndexViewModelUserItem(User u)
        {
            UserId = u.Id;
            LDAPName = u.LDAPName;
            Rank = u?.Rank?.Short ?? "-";
            LastName = u.NameFactory.Last;
            FirstName = u.NameFactory.First;
            IdNumber = u.IdNumber;
            Email = u.Email;
            Phone = u.ContactNumber;
            BlueDeckId = u.BlueDeckId;
        }
    }
}
