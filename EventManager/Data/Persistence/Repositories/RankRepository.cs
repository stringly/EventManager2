using EventManager.Data.Core.Repositories;
using EventManager.Models.Domain;
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
    }
}
