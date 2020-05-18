using EventManager.Data.Core.Repositories;
using EventManager.Models.Domain;
using Microsoft.AspNetCore.Http;
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
        public async Task<User> GetUserByLDAPNameAsync(string LDAPName)
        {
            return await EventManagerContext.Users.FirstOrDefaultAsync(x => x.LDAPName == LDAPName);
        }
        public User GetUserByLDAPName(string LDAPName)
        {
            return EventManagerContext.Users.FirstOrDefault(x => x.LDAPName == LDAPName);
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
        public async Task<SelectList> GetUserSelectListAsync()
        {
            return new SelectList(await EventManagerContext.Users.ToListAsync(), nameof(User.Id), nameof(User.DisplayName));
        }
        public async Task<IEnumerable<User>> GetUsersWithRankAsync(string searchString = "", int selectedRankId = 0, int page = 1, int pageSize = 25)
        {
            return await EventManagerContext.Users
                .Where(x => (string.IsNullOrEmpty(searchString) || (x.LDAPName.ToLower().Contains(searchString) || x.IdNumber.ToLower().Contains(searchString) || x.Email.ToLower().Contains(searchString))) && (selectedRankId == 0 || x.RankId == selectedRankId))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Rank)
                .Include(x => x.NameFactory)
                .ToListAsync();
        }

        public IEnumerable<User> GetUsersWithRank(string searchString = "", int selectedRankId = 0, int page = 1, int pageSize = 25)
        {
            return EventManagerContext.Users
                .Where(x => (string.IsNullOrEmpty(searchString) || (x.LDAPName.ToLower().Contains(searchString) || x.IdNumber.ToLower().Contains(searchString) || x.Email.ToLower().Contains(searchString))) && (selectedRankId == 0 || x.RankId == selectedRankId))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Rank)
                .Include(x => x.NameFactory);                
        }
        public async Task<IEnumerable<User>> GetUsersByRankIdAsync(int rankId)
        {
            if (rankId == 0)
            {
                throw new ArgumentException($"No Rank with id {rankId} exists.", nameof(rankId));
            }
            return await EventManagerContext.Users
                .Where(x => x.RankId == rankId)
                    .Include(x => x.Rank)
                .ToListAsync();
        }
        public IEnumerable<User> GetUsersByRankId(int rankId)
        {
            if (rankId == 0)
            {
                throw new ArgumentException($"No Rank with id {rankId} exists.", nameof(rankId));
            }
            return EventManagerContext.Users
                .Where(x => x.RankId == rankId)
                    .Include(x => x.Rank)
                .ToList();
        }

        public async Task<int> GetCountOfUsersInRankByRankIdAsync(int rankId)
        {
            return await EventManagerContext.Users.CountAsync(x => x.RankId == rankId);
        }

        public int GetCountOfUsersInRankByRankId(int rankId)
        {
            return EventManagerContext.Users.Count(x => x.RankId == rankId);
        }
        public async Task<User> GetUserWithOwnedEventsAndRegistrationsAsync(int id)
        {
            return await EventManagerContext.Users
                .Include(x => x.Rank)
                .Include(x => x.OwnedEvents)
                    .ThenInclude(x => x.EventType)
                .Include(x => x.OwnedEvents)
                    .ThenInclude(x => x.Registrations)
                .Include(x => x.Registrations)
                    .ThenInclude(x => x.Event)
                        .ThenInclude(x => x.EventType)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public User GetUserWithOwnedEventsAndRegistrations(int id)
        {
            return EventManagerContext.Users
                .Include(x => x.Rank)
                .Include(x => x.OwnedEvents)
                    .ThenInclude(x => x.EventType)
                .Include(x => x.OwnedEvents)
                    .ThenInclude(x => x.Registrations)
                .Include(x => x.Registrations)
                    .ThenInclude(x => x.Event)
                        .ThenInclude(x => x.EventType)
                .FirstOrDefault(x => x.Id == id);
        }

        public EventManagerContext EventManagerContext {
            get { return context as EventManagerContext; }
        }
    }
}
