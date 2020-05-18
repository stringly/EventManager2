using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class RankIndexViewModel : IndexViewModel
    {
        public RankIndexViewModel(IEnumerable<Rank> ranks, string sortOrder, string searchString, int totalItems = 0, int page = 1, int pageSize = 25)
        {
            PagingInfo = new PagingInfo { 
                ItemsPerPage = pageSize,
                CurrentPage = page,
                TotalItems = totalItems
            };
            CurrentSort = sortOrder;
            CurrentFilter = searchString;
            Ranks = ranks.ToList().ConvertAll(x => new RankIndexViewModelRankItem(x));
            ApplySort(sortOrder);
            //ApplyFilter(searchString);

        }
        public string RankIdSort { get; private set; }
        public string RankNameSort { get; private set; }
        public List<RankIndexViewModelRankItem> Ranks { get; private set; }

        public void ApplySort(string sortOrder)
        {
            switch (sortOrder)
            {
                case "RankName":
                    Ranks = Ranks.OrderBy(x => x.RankFullName).ToList();
                    break;
                case "rankName_desc":
                    Ranks = Ranks.OrderByDescending(x => x.RankFullName).ToList();
                    break;
                case "rankId_desc":
                    Ranks = Ranks.OrderByDescending(x => x.RankId).ToList();
                    break;
                default:
                    Ranks = Ranks.OrderBy(x => x.RankId).ToList();
                    break;
            }
            RankIdSort = String.IsNullOrEmpty(sortOrder) ? "rankId_desc" : "";
            RankNameSort = sortOrder == "RankName" ? "rankName_desc" : "RankName";
        }
        public void ApplyFilter(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                char[] arr = searchString.ToCharArray();
                arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c)
                                  || char.IsWhiteSpace(c)
                                  || c == '-')));
                string lowerString = new string(arr);
                lowerString = lowerString.ToLower();
                Ranks = Ranks
                    .Where(x => x.RankFullName.ToLower().Contains(lowerString) || x.Abbreviation.ToLower().Contains(lowerString))
                    .ToList();
            }
        }
    }

    public class RankIndexViewModelRankItem
    {
        public int RankId { get; private set; }
        public string Abbreviation { get; private set; }
        public string RankFullName { get; private set; }
        public int UserCount { get; private set; }

        public RankIndexViewModelRankItem(Rank r)
        {
            if (r == null)
            {
                throw new ArgumentNullException("Cannot construct item from null Rank object", nameof(r));
            }
            else if (r.Users == null)
            {
                throw new ArgumentNullException("Cannot construct item from Rank object with null User collection", nameof(r.Users));
            }
            else
            {
                RankId = r.Id;
                Abbreviation = r.Short;
                RankFullName = r.Full;
                UserCount = r.Users.Count();
            }
        }
    }
}
