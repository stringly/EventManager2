using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetUsersByRankAsync(int rank = 0);
        Task<IEnumerable<User>> GetUsersWithRegistrationsAsync();        
    }
}
