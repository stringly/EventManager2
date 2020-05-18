using EventManager.Data.Core;
using EventManager.Data.Core.Repositories;
using EventManager.Data.Core.Services;
using EventManager.Data.Persistence;
using EventManager.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace EventManager.Data
{
    public class DataInitializer
    {
        private readonly EventManagerContext _context;
        
        public DataInitializer(EventManagerContext context)
        {
            _context = context;
        }
        public void SeedData()
        {
            
            SeedUserRanks();
            _context.SaveChanges();
            SeedAdminUser();
            SeedEventTypes();
            _context.SaveChanges();
        }
        public void SeedUserRanks()
        {
            bool ranksPresent = _context.Ranks.Any();
            if (!ranksPresent)
            {
                List<Rank> ranks = new List<Rank>()
                {
                    new Rank("P/O", "Police Officer"),
                    new Rank("POFC", "Police Officer First Class"),
                    new Rank("Cpl.", "Corporal"),
                    new Rank("Sgt.", "Sergeant"),
                    new Rank("Lt", "Lieutenant"),
                    new Rank("Capt.", "Captain"),
                    new Rank("Maj.", "Major"),
                    new Rank("D/C", "Deputy Chief"),
                    new Rank("A/C", "Assistant Chief"),
                    new Rank("Chief", "Chief of Police"),
                    new Rank("A/Sgt.", "Acting Sergeant"),
                    new Rank("A/Lt.", "Acting Lieutenant"),
                    new Rank("A/Capt.", "Acting Captain"),
                    new Rank("A/Maj.", "Acting Major"),
                    new Rank("A/DC", "Acting Deputy Chief")
                };
                _context.Ranks.AddRange(ranks);
            }
        }
        public void SeedAdminUser()
        {
            bool usersPresent = _context.Users.Any();
            if (!usersPresent)
            {
                Rank adminUserRank = _context.Ranks.Where(x => x.Full == "Lieutenant").FirstOrDefault();
                if (adminUserRank == null)
                {
                    throw new KeyNotFoundException("Admin rank Lieutenant does not match any rank in the context.");
                }
                List<User> admins = new List<User>()
                {                    
                    new User("jcs30", 1, "Jason","Smith","3134", "jcsmith1@co.pg.md.us", "3016483444", adminUserRank),                 
                    new User("jcsmith1", 1, "Jason","Smith","3134", "jcsmith1@co.pg.md.us", "3016483444", adminUserRank)

                };
                _context.Users.AddRange(admins);
            }
        }

        public void SeedEventTypes()
        {
            bool eventTypesPresent = _context.EventTypes.Any();
            if (!eventTypesPresent)
            {
                List<EventType> eventTypes = new List<EventType>()
                {
                    new EventType("Training"),
                    new EventType("Overtime"),
                    new EventType("Special Assignment"),
                    new EventType("Meeting") 
                };
                _context.EventTypes.AddRange(eventTypes);
            }
        }
    }
}
