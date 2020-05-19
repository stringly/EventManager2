using System;
using System.Collections.Generic;
using System.Linq;

namespace EventManager.Models.Domain
{
    public class EventModule : IEntity
    {
        private EventModule()
        {
        }
        public EventModule(string title, string description)
        {
            UpdateTitle(title);
            UpdateDescription(description);
        }

        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int EventId { get; private set; }
        public virtual Event Event { get; private set; }
        private ICollection<Attendance> _attendance;
        public IEnumerable<Attendance> Attendance => _attendance?.ToList() ?? null;

        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                throw new ArgumentNullException("Cannot update Module: Title must not be null or empty string", nameof(newTitle));
            }
            Title = newTitle;
        }
        public void UpdateDescription(string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
            {
                throw new ArgumentNullException("Cannot update Module: Description must not be null or empty string", nameof(newDescription));
            }
            Description = newDescription;
        }
        public void AddAttendanceRecord(Attendance attendance)
        {
            EnsureAttendanceLoaded();
            if(_attendance.Any(x => x.AttendeeId == attendance.AttendeeId))
            {
                throw new ArgumentException("Cannot add Module attendance record: User has an existing attendance record for this module");
            }
            _attendance.Add(attendance);
        }
        public void RemoveAttendanceRecord(int id)
        {
            EnsureAttendanceLoaded();
            Attendance toRemove = _attendance.FirstOrDefault(x => x.Id == id);
            if (toRemove == null)
            {
                throw new ArgumentException($"Cannot remove Module attendance record: No attendance record with id {id} was found.");
            }
            _attendance.Remove(toRemove);
        }
        public void RemoveAttendanceRecordsByRange(IEnumerable<Attendance> toRemove)
        {
            EnsureAttendanceLoaded();
            foreach(Attendance a in toRemove)
            {
                _attendance.Remove(a);
            }            
        }
        private void EnsureAttendanceLoaded()
        {
            if (_attendance == null)
            {
                throw new NullReferenceException("You must load the Event Module's Attendance collection first.");
            }
        }

        

    }
}
