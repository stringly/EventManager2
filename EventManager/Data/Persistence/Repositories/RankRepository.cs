using EventManager.Data.Core.Repositories;
using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Persistence.Repositories
{
    public class RankRepository : Repository<Rank>, IRankRepository
    {
        public RankRepository(EventManagerContext context)
            : base(context)
        {
        }        
        public EventManagerContext EventManagerContext {
            get { return context as EventManagerContext; }
        }
        public IEnumerable<Rank> GetRanksWithUsers(string searchString = "", int page = 1, int pageSize = 25)
        {
        
            return EventManagerContext.Ranks
                .Where(x => (string.IsNullOrEmpty(searchString) || (x.FullName.ToLower().Contains(searchString) || x.ShortName.ToLower().Contains(searchString))))
                .Skip((page-1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Users)
                    .ThenInclude(x => x.Rank);
        }        

        public async Task<IEnumerable<Rank>> GetRanksWithUsersAsync(string searchString = "", int page = 1, int pageSize = 25)
        {
            return await EventManagerContext.Ranks
                .Where(x => (string.IsNullOrEmpty(searchString) || (x.FullName.ToLower().Contains(searchString) || x.ShortName.ToLower().Contains(searchString))))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Users)
                    .ThenInclude(x => x.Rank)
                .ToListAsync();
        }
        public async Task<Rank> GetRankByFullNameAsync(string rankFullName)
        {
            if (string.IsNullOrWhiteSpace(rankFullName))
            {
                throw new ArgumentException("RankFullName parameter cannot be empty string", nameof(rankFullName));
            }
            else
            {
                return await EventManagerContext.Ranks.FirstOrDefaultAsync(x => x.Full == rankFullName);
            }
        }
        public Rank GetRankByFullName(string rankFullName)
        {
            if (string.IsNullOrWhiteSpace(rankFullName))
            {
                throw new ArgumentException("RankFullName parameter cannot be empty string", nameof(rankFullName));
            }
            else
            {
                return EventManagerContext.Ranks.FirstOrDefault(x => x.Full == rankFullName);
            }
        }
        public SelectList GetRankSelectList()
        {
            return new SelectList(EventManagerContext.Ranks, nameof(Rank.Id), nameof(Rank.Full));
        }
        public async Task<SelectList> GetRankSelectListAsync()
        {
            return new SelectList(await EventManagerContext.Ranks.ToListAsync(), nameof(Rank.Id), nameof(Rank.Full));
        }

        public async Task<Rank> GetRankWithUsersAsync(int rankId)
        {
            return await EventManagerContext.Ranks
                .Include(x => x.Users)
                    .ThenInclude(x => x.Rank)
                .FirstOrDefaultAsync(x => x.Id == rankId);                
        }

        public Rank GetRankWithUsers(int rankId)
        {
            return EventManagerContext.Ranks
                .Include(x => x.Users)
                    .ThenInclude(x => x.Rank)
                .FirstOrDefault(x => x.Id == rankId);
        }
    }
}
