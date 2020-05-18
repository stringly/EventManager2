using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByLDAPNameAsync(string LDAPName);
        User GetUserByLDAPName(string LDAPName);
        Task<IEnumerable<User>> GetUsersWithRankAsync(string searchString = "", int selectedRankId = 0, int page = 1, int pageSize = 25);
        IEnumerable<User> GetUsersWithRank(string searchString = "", int selectedRankId = 0, int page = 1, int pageSize = 25);
        Task<IEnumerable<User>> GetUsersWithRegistrationsAsync();
        Task<IEnumerable<User>> GetUsersByRankIdAsync(int rankId);
        IEnumerable<User> GetUsersByRankId(int rankId);
        Task<int> GetCountOfUsersInRankByRankIdAsync(int rankId);
        int GetCountOfUsersInRankByRankId(int rankId);
        Task<User> GetUserWithOwnedEventsAndRegistrationsAsync(int id);
        User GetUserWithOwnedEventsAndRegistrations(int id);
        SelectList GetUserSelectList();
        Task<SelectList> GetUserSelectListAsync();
    }
}
