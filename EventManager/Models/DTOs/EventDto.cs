using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.DTOs
{
    public class EventDto
    {
        public int EventId { get; set; }
        public string EventTitle { get; set; }
        public string EventType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int EventSeriesId { get; set; }
        public string EventSeriesTitle { get; set; }
        public int CreatorId { get;set;}
        public string CreatorName { get; set; }
        public string CreatorEmail { get; set; }
        public string Status { get; set; }
    }
}
