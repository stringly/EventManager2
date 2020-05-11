using System;

namespace EventManager.Models.Domain
{
    public class Registration
    {
        private Registration() { }
        public Registration(User u, Event e, RegistrationStatus status){
            if (u == null)
            {
                throw new ArgumentNullException("User parameter cannot be null.", nameof(u));
            }
            else if (e == null)
            {
                throw new ArgumentNullException("Event parameter cannot be null", nameof(e));
            }            
            else
            {
                TimeStamp = DateTime.Now;
                UserId = u.Id;
                User = u;
                Event = e;
                EventId = e.Id;
                Status = status;
            }
        }
        public int Id { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public int? UserId { get; private set; }
        public virtual User User { get; private set; }
        public int EventId { get; private set; }
        public virtual Event Event { get; private set; }
        public RegistrationStatus Status { get; private set; }
        public string UserDisplayName => User?.DisplayName ?? "-";
        public string UserEmail => User?.Email ?? "-";
        public string UserContactNumber => User?.ContactNumber ?? "-";
        public bool IsActive => Status == RegistrationStatus.Accepted;        
        public bool IsStandy => Status == RegistrationStatus.Standy;        
        public bool IsPending => Status == RegistrationStatus.Pending;
        public bool IsRejected => Status == RegistrationStatus.Rejected;                

        public void UpdateStatus(RegistrationStatus newStatus)
        {
            Status = newStatus;
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
