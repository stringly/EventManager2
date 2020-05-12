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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(EventManagerContext context)
            : base(context)
        {
        }
        public async Task<IEnumerable<User>> GetUsersWithRegistrationsAsync()
        {
            return await EventManagerContext.Users
                .Include(x => x.Rank)
                .Include(x => x.Registrations)
                    .ThenInclude(x => x.Event)
                .ToListAsync();
        }
        public SelectList GetUserSelectList()
        {
            return new SelectList(EventManagerContext.Users, nameof(User.Id), nameof(User.DisplayName));
        }

        public async Task<IEnumerable<User>> GetUsersWithRankAsync(int selectedRankId = 0, int page = 1, int pageSize = 25)
        {
            return await EventManagerContext.Users
                .Where(x => (selectedRankId == 0 || x.RankId == selectedRankId))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Rank)
                .ToListAsync();
        }

        public IEnumerable<User> GetUsersWithRank(int selectedRankId = 0, int page = 1, int pageSize = 25)
        {
            return EventManagerContext.Users
                .Where(x => (selectedRankId == 0 || x.RankId == selectedRankId))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Rank);                
        }

        public EventManagerContext EventManagerContext {
            get { return context as EventManagerContext; }
        }
    }
}
