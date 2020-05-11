using EventManager.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.Domain
{
    public class Rank : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Short { get; set; }
        public string Full { get; set; }
        [NotMapped]
        public string FullName { get {
                if (!String.IsNullOrEmpty(this.Full))
                {
                    return Full;
                }
                else
                {
                    return "Mr./Ms.";
                }
            } 
        }
        [NotMapped]
        public string ShortName { get {
                if (!String.IsNullOrEmpty(Short))
                {
                    return Short;
                }
                else
                {
                    return "Mr./Ms.";
                }
            } 
        }
        public Rank()
        {
        }
        public Rank(string Abbreviation, string FullName)
        {
            Short = Abbreviation;
            Full = FullName;
        }



    }
}
