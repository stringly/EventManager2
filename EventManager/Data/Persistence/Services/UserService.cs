using EventManager.Data.Core.Repositories;
using EventManager.Data.Core.Services;
using EventManager.Data.Persistence.Repositories;
using EventManager.Models.Domain;
using EventManager.sharedkernel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Data.Persistence.Services
{
    public class UserService : IUserService
    {
        public UserService(EventManagerContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            Users = new UserRepository(_context);
            Ranks = new RankRepository(_context);
            Events = new EventRepository(_context);
            Registrations = new RegistrationRepository(_context);
            _httpContextAccessor = httpContextAccessor;
            _currentUser = Users.GetUserByLDAPName(_httpContextAccessor.HttpContext.User.Identity.Name.Split('\\')[1]);
        }
        private EventManagerContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly User _currentUser;
        public IUserRepository Users { get; private set; }
        public IRankRepository Ranks { get; private set; }
        public IEventRepository Events { get; private set; }
        public IRegistrationRepository Registrations { get; private set;}
        public bool CreateUser(string LDAPName, uint blueDeckId, string firstName, string lastName, string idNumber, string email, string contactNumber, int rankId, out string response)
        {
            if (Users.ValueIsInUseByIdForExpression(x => x.LDAPName == LDAPName))
            {
                response = "Cannot create User: LDAP Name is in use by another User.";
                return false;
            }
            else if (Users.ValueIsInUseByIdForExpression(x => x.BlueDeckId == blueDeckId))
            {
                response = "Cannot create User: BlueDeckId is in use by another User.";
                return false;
            }
            else if (Users.ValueIsInUseByIdForExpression(x => x.IdNumber == idNumber))
            {
                response = "Cannot create User: IdNumber is in use by another User.";
                return false;
            }
            else if (Users.ValueIsInUseByIdForExpression(x => x.Email == email))
            {
                response = "Cannot create User: Email is in use by another User.";
                return false;
            }
            else
            {
                Rank r = Ranks.Get(rankId);
                if(r == null)
                {
                    response = $"Cannot create User: no Rank with id {rankId} could be found";
                    return false;
                }                
                try
                {
                    User toAdd = new User(LDAPName, blueDeckId, firstName, lastName, idNumber, email, contactNumber, r);
                    Users.Add(toAdd);
                    Complete();
                    response = "User successfully created";
                    return true;
                }
                catch(Exception ex)
                {
                    response = ex.Message;
                    return false;
                }
            }
        }
        public bool RemoveUser(int id, out string response)
        {
            User toRemove = Users.Get(id);
            if(toRemove == null)
            {
                response = "Cannot remove User: User not found.";
                return false;
            }
            else if (Events.Find(x => x.OwnerId == id).Any())
            {
                response = "Cannot remove User: User has ownership of Events. Owned Events must be reassigned before User can be deleted.";
                return false;
            }
            else if (Registrations.Find(x => x.UserId == id).Any())
            {
                response = "Cannot remove User: User has Registrations. Registrations must be removed before the User can be removed.";
                return false;
            }
            else
            {
                try
                {
                    Users.Remove(toRemove);
                    Complete();
                    response = "User succesfully deleted.";
                    return true;
                }
                catch (Exception ex)
                {
                    response = ex.Message;
                    return false;
                }
            }
        }
        public bool UpdateUser(int id, string LDAPName, uint blueDeckId, string firstName, string lastName, string idNumber, string email, string contactNumber, int rankId, out string response)
        {
            if (Users.ValueIsInUseByIdForExpression(x => x.LDAPName == LDAPName && x.Id != id))
            {
                response = "Cannot create User: LDAP Name is in use by another User.";
                return false;
            }
            else if (Users.ValueIsInUseByIdForExpression(x => x.BlueDeckId == blueDeckId && x.Id != id))
            {
                response = "Cannot create User: BlueDeckId is in use by another User.";
                return false;
            }
            else if (Users.ValueIsInUseByIdForExpression(x => x.IdNumber == idNumber && x.Id != id))
            {
                response = "Cannot create User: IdNumber is in use by another User.";
                return false;
            }
            else if (Users.ValueIsInUseByIdForExpression(x => x.Email == email && x.Id != id))
            {
                response = "Cannot create User: Email is in use by another User.";
                return false;
            }
            else
            {
                User toUpdate = Users.Get(id);
                if (toUpdate == null)
                {
                    response = "Cannot update User: UserId was not found.";
                    return false;
                }
                Rank newRank = Ranks.Get(rankId);
                if(newRank == null)
                {
                    response = $"Cannot update User: Rank with id {rankId} was not found.";
                    return false;
                }
                try
                {
                    if (toUpdate.LDAPName != LDAPName) { toUpdate.UpdateLDAPName(LDAPName); }
                    if (toUpdate.BlueDeckId != blueDeckId) { toUpdate.UpdateBlueDeckId(blueDeckId); }
                    if (toUpdate.NameFactory.First != firstName || toUpdate.NameFactory.Last != lastName) {toUpdate.UpdateName(PersonFullName.Create(firstName, lastName));}
                    if (toUpdate.IdNumber != idNumber) { toUpdate.UpdateIdNumber(idNumber); }
                    if (toUpdate.Email != email) { toUpdate.UpdateEmail(email); }
                    if (toUpdate.ContactNumber != contactNumber) { toUpdate.UpdateContactNumber(contactNumber); }
                    if (toUpdate.RankId != newRank.Id) { toUpdate.UpdateRank(newRank); }
                    Complete();
                    response = "User successfully updated";
                    return true;
                }
                catch(Exception ex)
                {
                    response = ex.Message;
                    return false;
                }
            }
        }
        public bool CreateRank(string abbreviation, string fullName, out string response)
        {
            if(Ranks.ValueIsInUseByIdForExpression(x => x.Short == abbreviation || x.Full == fullName))
            {
                response = "Cannot create Rank: Abbreviation or Full Name is in use by another Rank";
                return false;
            }            
            try
            {
                Rank toAdd = new Rank(abbreviation, fullName);
                Ranks.Add(toAdd);
                Complete();
                response = "Rank successfully added.";
                return true;
            }
            catch (Exception ex)
            {
                response = ex.Message;
                return false;
            }
        }
        public bool UpdateRank(int id, string abbreviation, string fullName, out string response)
        {
            if(Ranks.ValueIsInUseByIdForExpression(x => (x.Short == abbreviation || x.FullName == fullName) && x.Id != id))
            {
                response = "Cannot update Rank: Abbreviation or Full Name is in use by another Rank";
                return false;
            }            
            try
            {
                Rank toUpdate = Ranks.Get(id);
                toUpdate.UpdateAbbreviation(abbreviation);
                toUpdate.UpdateFullName(fullName);
                Complete();
                response = "Rank successfully updated.";
                return true;
            }
            catch (Exception ex)
            {
                response = ex.Message;
                return false;
            }
        }
        public bool DeleteRank(int id, out string response)
        {
            if (id == 0 || !Ranks.Exists(id))
            {
                throw new ArgumentException($"No Rank with id {id} could be found.", nameof(id));
            }
            if (Users.GetCountOfUsersInRankByRankId(id) > 0)
            {
                response = "Cannot delete Rank with active Users.";
                return false;
            }            
            try
            {
                Rank toRemove = Ranks.Get(id);
                Ranks.Remove(toRemove);
                Complete();
                response = "Rank successfully deleted.";
                return true;
            }
            catch (Exception ex)
            {
                response = ex.Message;
                return false;
            }
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
