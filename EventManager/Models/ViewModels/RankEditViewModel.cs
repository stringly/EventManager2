using EventManager.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class RankEditViewModel
    {
        public RankEditViewModel()
        {
        }
        public RankEditViewModel(Rank r)
        {
            if (r == null)
            {
                throw new ArgumentNullException("Cannot create viewmodel with null Rank object.", nameof(r));
            }
            else if (r.Users == null)
            {
                throw new ArgumentNullException("Cannot create viewmodel from Rank object with null Users collection.", nameof(r.Users));
            }
            Id = r.Id;
            Short = r.Short;
            Long = r.Full;
            InitializeUserList(r.Users);
        }
        [Display(Name = "Rank Id"), HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Rank Abbreviation"), Required, StringLength(10)]
        public string Short { get; set; }
        [Display(Name = "Rank Full Name"), Required, StringLength(25)]
        public string Long { get; set; }
        public IEnumerable<UserIndexViewModelUserItem> Users { get; private set;}
        public void InitializeUserList(IEnumerable<User> users)
        {
            Users = users.ToList().ConvertAll(x => new UserIndexViewModelUserItem(x));
        }
    }
}