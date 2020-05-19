using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.Domain
{
    public class Attendance : IEntity
    {
        private Attendance() { }
        public Attendance(User u, Event e, AttendanceStatus status)
        {
            if (u == null || u.Id == 0)
            {
                throw new ArgumentNullException("Cannot create Attendance record: User object is null/invalid", nameof(u));
            }
            else if(e == null || e.Id == 0)
            {
                throw new ArgumentNullException("Cannot create Attendance record: Event object is null/invalid", nameof(u));
            }
            AttendeeId = u.Id;
            EventId = u.Id;
            Status = status;
        }
        public Attendance(User u, EventModule em, AttendanceStatus status)
        {
            if (u == null || u.Id == 0)
            {
                throw new ArgumentNullException("Cannot create Attendance record: User object is null/invalid", nameof(u));
            }
            else if (em == null || em.Id == 0)
            {
                throw new ArgumentNullException("Cannot create Attendance record: EventModule object is null/invalid", nameof(u));
            }
            AttendeeId = u.Id;
            EventId = u.Id;
            Status = status;
        }
        public int Id { get; private set;}
        public int? AttendeeId { get; private set; }
        public virtual User Attendee { get; private set; }
        public int EventId { get; private set; }
        public virtual Event Event { get; private set; }
        public int? ModuleId { get; private set; }
        public virtual EventModule Module { get; private set; }
        public AttendanceStatus Status { get; private set; }
        public void UpdateStatus(AttendanceStatus status)
        {
            Status = status;
        }
        public void Absent()
        {
            Status = AttendanceStatus.Absent;
        }
        public void Present()
        {
            Status = AttendanceStatus.Present;
        }
        public void Excused()
        {
            Status = AttendanceStatus.Excused;
        }
        public void Pass()
        {
            Status = AttendanceStatus.Pass;
        }
        public void Fail()
        {
            Status = AttendanceStatus.Fail;
        }

        public enum AttendanceStatus
        {
            Absent,
            Present,
            Excused,
            Pass,
            Fail
        }
    }
    
}
