using EventManager.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Models.Domain
{
    public class Registration : IEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public int? UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserEmail { get; set; }
        public virtual User User { get; set; }
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
