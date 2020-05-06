using EventManager.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.Domain
{
    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string UserId { get; set; }
        public virtual EventManagerUser User { get; set; }
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
        public RegistrationStatus Status { get; set; }

        [NotMapped]
        public bool IsActive { get {
                return Status == RegistrationStatus.Accepted;
            } 
        }
        [NotMapped]
        public bool IsStandy { get {
                return Status == RegistrationStatus.Standy;
            } 
        }
        [NotMapped]
        public bool IsPending { get {
                return Status == RegistrationStatus.Pending;
            }
        }
        [NotMapped]
        public bool IsRejected { get {
                return Status == RegistrationStatus.Rejected;
            }
        }

        public enum RegistrationStatus
        {
            Pending,
            Accepted,
            Standy,
            Rejected
        }

    }
}
