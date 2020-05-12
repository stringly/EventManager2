using EventManager.Data.Core;
using EventManager.Data.Core.Repositories;
using EventManager.Data.Persistence;
using EventManager.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace EventManager.Data
{
    public static class DataInitializer
    {
        private static IUnitOfWork _unitOfWork;
        public static void SeedData(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            SeedUserRanks();
            unitOfWork.Complete();
            SeedAdminUser();
            SeedEventTypes();
            unitOfWork.Complete();
        }
        public static void SeedUserRanks()
        {
            bool ranksPresent = _unitOfWork.Ranks.Any();
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
                _unitOfWork.Ranks.AddRange(ranks);
            }
        }
        public static void SeedAdminUser()
        {
            bool usersPresent = _unitOfWork.Users.Any();
            if (!usersPresent)
            {
                Rank adminUserRank = _unitOfWork.Ranks.GetRankByFullName("Lieutenant");
                if (adminUserRank == null)
                {
                    throw new KeyNotFoundException("Admin rank Lieutenant does not match any rank in the context.");
                }
                List<User> admins = new List<User>()
                {                    
                    new User("jc30", 1, "Jason","Smith","3134", "jcsmith1@co.pg.md.us", "3016483444", adminUserRank),                 
                    new User("jcsmith1", 1, "Jason","Smith","3134", "jcsmith1@co.pg.md.us", "3016483444", adminUserRank)

                };
                _unitOfWork.Users.AddRange(admins);
            }
        }

        public static void SeedEventTypes()
        {
            bool eventTypesPresent = _unitOfWork.EventTypes.Any();
            if (!eventTypesPresent)
            {
                List<EventType> eventTypes = new List<EventType>()
                {
                    new EventType("Training"),
                    new EventType("Overtime"),
                    new EventType("Special Assignment"),
                    new EventType("Meeting") 
                };
                _unitOfWork.EventTypes.AddRange(eventTypes);
            }
        }
    }
}
