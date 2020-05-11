using EventManager.Data.Core.Repositories;
using System;

namespace EventManager.Data.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IEventRepository Events { get; }
        IEventSeriesRepository EventSeries { get; }
        IEventTypeRepository EventTypes { get; }
        IRankRepository Ranks { get; }
        IUserRepository Users { get; }        
        IRegistrationRepository Registrations { get; }
        int Complete();
    }
}
