using EventManager.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EventManager.Models.Domain
{
    public class EventSeries : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get;set;}
        public string Description { get; set;}
        public virtual ICollection<Event> Events { get; set; }

        public List<Event> AvailableEvents()
        {
            return Events?.Where(x => x.IsOpenForRegistrations() == true).ToList() ?? new List<Event>();
        }
        public bool HasAvailableEvents()
        {
            return Events?.Any(x => x.IsOpenForRegistrations() == true) ?? false;
        }
    }
}
