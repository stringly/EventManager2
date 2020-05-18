using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class UserAddViewModel
    {
        public UserAddViewModel()
        {
            Ranks = new SelectList(new List<SelectListItem>());
        }
        public UserAddViewModel(SelectList ranks)
        {
            InitializeRanksList(ranks);
        }
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
        public SelectList Ranks { get; private set; }
        public void InitializeRanksList(SelectList ranks)
        {
            if (ranks == null)
            {
                throw new ArgumentNullException("Cannot construct viewmodel with null Ranks collection");
            }
            Ranks = ranks;
        }

    }
}
