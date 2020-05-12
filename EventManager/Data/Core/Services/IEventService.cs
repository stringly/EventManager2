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
        bool CreateEvent(EventAddViewModel form, out string response);

    }
}
