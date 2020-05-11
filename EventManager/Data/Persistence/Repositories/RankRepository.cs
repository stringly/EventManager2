using EventManager.Data.Core.Repositories;
using EventManager.Models.Domain;
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
    }
}
