using EventManager.Data.Core.Repositories;
using EventManager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Services
{
    public interface IEventService : IDisposable
    {
        IEventRepository Events { get; }
        IEventSeriesRepository EventSeries { get; }
        IEventTypeRepository EventTypes { get; }
        IRankRepository Ranks { get; }
        IUserRepository Users { get; }
        IRegistrationRepository Registrations { get; }
        int Complete();
        bool CreateEvent(
            out string response,
            int eventTypeId, 
            int ownerUserId, 
            string title,
            string description,
            DateTime startDate,
            DateTime endDate,
            DateTime registrationOpenDate,
            DateTime? registrationClosedDate,
            string locationLine1,
            string locationLine2, 
            string locationCity,
            string locationState,
            string locationZip,
            int eventSeriesId = 0,
            int maxRegistrations = 1,
            int minRegistrations = 0,
            bool allowStandbyRegistrations = false,
            int maxStandbyRegistrations = 0,
            string fundCenter = ""
            );
        bool UpdateEvent(
            out string response,
            int eventId,
            int eventTypeId,
            int ownerUserId,
            string title,
            string description,
            DateTime startDate,
            DateTime endDate,
            DateTime registrationOpenDate,
            DateTime? registrationClosedDate,
            string locationLine1,
            string locationLine2,
            string locationCity,
            string locationState,
            string locationZip,
            int eventSeriesId = 0,
            int maxRegistrations = 1,
            int minRegistrations = 0,
            bool allowStandbyRegistrations = false,
            int maxStandbyRegistrations = 0,
            string fundCenter = "");
        bool DeleteEvent(int id, out string response);
        bool CreateEventSeries(string title, string description, out string response);
        bool UpdateEventSeries(int eventSeriesId, string title, string description, out string response);
        bool DeleteEventSeries(int id, out string response);
        bool CreateEventType(string eventTypeName, out string response);
        bool UpdateEventType(int eventTypeId, string eventTypeName, out string response);
        bool DeleteEventType(int id, out string response);
    }
}
