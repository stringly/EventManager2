using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Repositories
{
    public interface IRankRepository : IRepository<Rank>
    {
        Task<Rank> GetRankByFullNameAsync(string rankFullName);
        Rank GetRankByFullName(string rankFullName);
    }
}
