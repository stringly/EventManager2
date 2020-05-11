using EventManager.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventManager.Models.Domain
{
    public class EventType : IEntity
    {
        [Key]
        public int Id { get;set;}
        public string EventTypeName { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
