﻿using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Repositories
{
    public interface IRankRepository : IRepository<Rank>
    {        
        IEnumerable<Rank> GetRanksWithUsers(string searchString = "", int page = 1, int pageSize = 25 );
        Task<IEnumerable<Rank>> GetRanksWithUsersAsync(string searchString = "", int page = 1, int pageSize = 25);
        Task<Rank> GetRankWithUsersAsync(int rankId);
        Rank GetRankWithUsers(int rankId);
        Task<Rank> GetRankByFullNameAsync(string rankFullName);
        Rank GetRankByFullName(string rankFullName);
        SelectList GetRankSelectList();
        Task<SelectList> GetRankSelectListAsync();
    }
}
