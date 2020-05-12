using EventManager.Data.Core;
using EventManager.Data.Core.Repositories;
using EventManager.Data.Persistence.Repositories;
using EventManager.Models.Domain;

namespace EventManager.Data.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventManagerContext _context;        
        public IEventRepository Events { get; private set; }
        public IEventSeriesRepository EventSeries { get; private set; }
        public IEventTypeRepository EventTypes { get; private set;}
        public IRankRepository Ranks { get; private set; }
        public IUserRepository Users { get; private set; }
        
        public IRegistrationRepository Registrations { get; private set; }

        public UnitOfWork(EventManagerContext context)
        {
            _context = context;
            Events = new EventRepository(_context);
            EventSeries = new EventSeriesRepository(_context);
            EventTypes = new EventTypeRepository(_context);
            Ranks = new RankRepository(_context);
            Users = new UserRepository(_context);            
            Registrations = new RegistrationRepository(_context);
        }
        public int Complete()
        {
            return  _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
