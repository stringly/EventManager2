using EventManager.Data.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Core.Services
{
    public interface IUserService : IDisposable
    {
        IUserRepository Users { get; }
        IRankRepository Ranks { get; }
        IEventRepository Events { get; }
        IRegistrationRepository Registrations { get; }
        bool CreateUser(string LDAPName, uint blueDeckId, string firstName, string lastName, string idNumber, string email, string contactNumber, int RankId, out string response);
        bool UpdateUser(int id, string LDAPName, uint blueDeckId, string firstName, string lastName, string idNumber, string email, string contactNumber, int RankId, out string response);
        bool RemoveUser(int id, out string response);
        bool CreateRank(string abbreviation, string fullName, out string response);
        bool UpdateRank(int id, string abbreviation, string fullName, out string response);
        bool DeleteRank(int id, out string response);
        int Complete();
    }
}
