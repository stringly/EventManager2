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
        Task<IEnumerable<User>> GetUsersWithRankAsync(int selectedRankId = 0, int page = 1, int pageSize = 25);
        IEnumerable<User> GetUsersWithRank(int selectedRankId = 0, int page = 1, int pageSize = 25);
        Task<IEnumerable<User>> GetUsersWithRegistrationsAsync();
        SelectList GetUserSelectList();
    }
}
