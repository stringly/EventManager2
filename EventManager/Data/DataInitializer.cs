using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace EventManager.Data
{
    public static class DataInitializer
    {
        
        public static void SeedData(ApplicationDbContext context)
        {
            SeedUserRanks(context);
            context.SaveChangesAsync();
        }
        public static void SeedUserRanks(ApplicationDbContext context)
        {
            if (!context.Ranks.Any())
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
                context.Ranks.AddRangeAsync(ranks);
            }
        }
    }
}
