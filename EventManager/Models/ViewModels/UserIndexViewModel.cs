using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class UserIndexViewModel : IndexViewModel
    {
        public UserIndexViewModel(
            IEnumerable<User> users, 
            SelectList ranks,
            int selectedRankId,
            string sortOrder, 
            string searchString, 
            int totalItems = 0,
            int page = 1, 
            int pageSize = 25
            )
        {
            PagingInfo = new PagingInfo { 
                ItemsPerPage = 25,
                CurrentPage = page,
                TotalItems = totalItems
            };
            CurrentFilter = searchString;
            CurrentSort = sortOrder;
            SelectedRankId = selectedRankId;
            Users = users.ToList().ConvertAll(x => new UserIndexViewModelUserItem(x));
            Ranks = ranks;
            ApplySort(sortOrder);

        }
        public string LastNameSort { get; private set; }
        public string BadgeNumberSort { get; private set; }
        public string RankSort { get; private set; }
        public int SelectedRankId { get; private set; }
        public List<UserIndexViewModelUserItem> Users { get; private set; }
        public SelectList Ranks { get; private set; }
        public void ApplySort(string sortOrder)
        {
            switch (sortOrder)
            {
                case "BadgeNumber":
                    Users = Users.OrderBy(x => x.IdNumber).ToList();
                    break;
                case "badgeNumber_desc":
                    Users = Users.OrderByDescending(x => x.IdNumber).ToList();
                    break;
                case "Rank":
                    Users = Users.OrderBy(x => x.Rank).ToList();
                    break;
                case "rank_desc":
                    Users = Users.OrderByDescending(x => x.Rank).ToList();
                    break;
                case "lastName_desc":
                    Users = Users.OrderByDescending(x => x.LastName).ToList();
                    break;
                default:
                    Users = Users.OrderBy(x => x.LastName).ToList();
                    break;
            }
            LastNameSort = String.IsNullOrEmpty(sortOrder) ? "lastName_desc" : "";
            BadgeNumberSort = sortOrder == "BadgeNumber" ? "badgeNumber_desc" : "BadgeNumber";
            RankSort = sortOrder == "Rank" ? "rank_desc" : "Rank";
        }
        private void ApplyFilter(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                  || char.IsWhiteSpace(c)
                                  || c == '-')));
                string lowerString = new string(arr);
                lowerString = lowerString.ToLower();
                Users = Users
                    .Where(x => x.LastName.ToLower().Contains(lowerString)
                        || x.FirstName.ToLower().Contains(lowerString)
                        || x.Email.ToLower().Contains(lowerString)
                        || x.IdNumber.ToLower().Contains(lowerString))
                    .ToList();
            }
        }
        

    }

    public class UserIndexViewModelUserItem
    {
        public int UserId { get; private set; }
        public string LDAPName { get; private set; }
        public string Rank { get; private set; }
        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public string IdNumber { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public uint BlueDeckId { get; private set;}
        public UserIndexViewModelUserItem(User u)
        {
            if(u == null)
            {
                throw new ArgumentNullException("Cannot construct item from null User object", nameof(u));
            }
            else if (u.Rank == null)
            {
                throw new ArgumentNullException("Cannot construct item from User object with null Rank object", nameof(u.Rank));
            }
            else
            {
                UserId = u.Id;
                LDAPName = u.LDAPName;
                Rank = u.Rank.Short;
                LastName = u.NameFactory.Last;
                FirstName = u.NameFactory.First;
                IdNumber = u.IdNumber;
                Email = u.Email;
                Phone = u.ContactNumber;
                BlueDeckId = u.BlueDeckId;
            }            
        }
    }
}
