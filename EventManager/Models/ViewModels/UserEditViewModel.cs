using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class UserEditViewModel
    {
        public UserEditViewModel()
        {
            Ranks = new SelectList(new List<SelectListItem>());
        }
        public UserEditViewModel(User u, SelectList ranks)
        {
            if (u == null)
            {
                throw new ArgumentNullException("Cannot construct viewmodel from null User object.", nameof(u));
            }
            UserId = u.Id;
            FirstName = u.NameFactory.First;
            LastName = u.NameFactory.Last;
            LDAPName = u.LDAPName;
            BlueDeckId = (int)u.BlueDeckId;
            IdNumber = u.IdNumber;
            Email = u.Email;
            ContactNumber = u.ContactNumber;
            RankId = (int)u.RankId;
            RankName = u.Rank.FullName;
            InitializeRanksList(ranks);
            InitializeRegistrationsList(u.Registrations);
            InitializeOwnedEventsList(u.OwnedEvents);
        }
        [Display(Name = "User Id")]
        [HiddenInput]
        public int UserId { get;set; }
        [Display(Name = "First Name"), Required, StringLength(50)]
        public string FirstName { get; set; }
        [Display(Name = "Last Name"), Required, StringLength(50)]
        public string LastName { get; set; }
        [Display(Name = "Windows Name"), Required, StringLength(50)]
        public string LDAPName { get; set; }
        [Display(Name = "BlueDeck Id"), Required]
        public int BlueDeckId { get; set; }
        [Display(Name = "ID/Badge Number"), Required, StringLength(10)]
        public string IdNumber { get; set; }
        [Display(Name = "Email"), Required, RegularExpression(@"[A-Za-z0-9]+@co\.pg\.md\.us", ErrorMessage = "Must be a valid Email in the domain.")]
        public string Email { get; set; }
        [Display(Name = "Contact Number"), StringLength(10)]
        public string ContactNumber { get; set; }
        [Display(Name = "Rank"), Required]
        public int RankId { get; set; }
        public string RankName { get; set; }
        public SelectList Ranks { get; private set; }
        public IEnumerable<EventIndexViewModelEventItem> OwnedEvents { get; private set; }
        public IEnumerable<RegistrationIndexViewModelRegistrationItem> Registrations { get; private set;}
        public void InitializeRanksList(SelectList ranks)
        {
            if (ranks == null)
            {
                throw new ArgumentNullException("Cannot construct viewmodel with null Ranks collection.", nameof(ranks));
            }
            Ranks = ranks;
        }
        public void InitializeOwnedEventsList(IEnumerable<Event> events)
        {
            if (events == null)
            {
                throw new ArgumentNullException("Cannot construct viewmodel with null Owned Events collection.", nameof(events));
            }
            OwnedEvents = events.ToList().ConvertAll(x => new EventIndexViewModelEventItem(x));
        }
        public void InitializeRegistrationsList(IEnumerable<Registration> registrations)
        {
            if (registrations == null)
            {
                throw new ArgumentNullException("Cannot construct viewmodel with null Registrations collection.", nameof(registrations));
            }
            Registrations = registrations.ToList().ConvertAll(x => new RegistrationIndexViewModelRegistrationItem(x));
        }
    }
}
