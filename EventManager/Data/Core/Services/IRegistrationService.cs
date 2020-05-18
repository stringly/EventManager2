using EventManager.Data.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EventManager.Models.Domain.Registration;

namespace EventManager.Data.Core.Services
{
    public interface IRegistrationService
    {
        IEventRepository Events { get;}
        IRegistrationRepository Registrations { get;}
        IUserRepository Users { get;}        
        int Complete();
        bool CreateRegistration(int userId, int eventId, out string response);
        bool UpdateRegistrationStatus(int registrationId, RegistrationStatus status, out string response);
        bool DeleteRegistration(int registrationId, out string response);
        
    }
}
