using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.Domain
{
    public class EventSeries
    {
        [Key]
        public int EventSeriesId { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
