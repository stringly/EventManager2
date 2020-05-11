using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class RankIndexViewModel : IndexViewModel
    {
        public string RankIdSort { get; set; }
        public string RankNameSort { get; set; }
        public List<RankIndexViewModelRankItem> Ranks { get; private set; }

        public RankIndexViewModel(int pageSize)
        {
            PagingInfo = new PagingInfo { ItemsPerPage = pageSize };
        }
        public void InitializeRankList(List<Rank> ranks, int page)
        {
            PagingInfo.TotalItems = ranks.Count();
            PagingInfo.CurrentPage = page;
            Ranks = ranks
                .Skip((PagingInfo.CurrentPage - 1) * PagingInfo.ItemsPerPage)
                .Take(PagingInfo.ItemsPerPage)
                .ToList()
                .ConvertAll(x => new RankIndexViewModelRankItem(x));
            
        }
    }

    public class RankIndexViewModelRankItem
    {
        public int RankId { get;set;}
        public string Abbreviation { get; set; }
        public string RankFullName { get; set; }

        public RankIndexViewModelRankItem(Rank r)
        {
            RankId = r.Id;
            Abbreviation = r.Short;
            RankFullName = r.Full;
        }
    }
}
