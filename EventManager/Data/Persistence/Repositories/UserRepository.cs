using EventManager.Data.Core.Repositories;
using EventManager.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(EventManagerContext context)
            : base(context)
        {
        }
        public async Task<IEnumerable<User>> GetUsersByRankAsync(int filterByRankId = 0)
        {
            return await EventManagerContext.Users
                .Where(x => (filterByRankId == 0 || x.RankId == filterByRankId))
                .Include(x => x.Rank)
                .ToListAsync();
        }
        public async Task<IEnumerable<User>> GetUsersWithRegistrationsAsync()
        {
            return await EventManagerContext.Users
                .Include(x => x.Rank)
                .Include(x => x.Registrations)
                    .ThenInclude(x => x.Event)
                .ToListAsync();
        }
        public EventManagerContext EventManagerContext {
            get { return context as EventManagerContext; }
        }
    }
}
