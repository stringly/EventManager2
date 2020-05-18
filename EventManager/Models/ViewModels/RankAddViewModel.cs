using EventManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public class RankAddViewModel
    {
        public RankAddViewModel()
        {
        }
        public RankAddViewModel(Rank r)
        {
            if(r == null)
            {
                throw new ArgumentNullException("Cannot create viewmodel with null Rank object.", nameof(r));
            }
            Short = r.Short;
            Long = r.Full;
        }
        [Display(Name = "Rank Abbreviation"), Required, StringLength(10)]
        public string Short { get; set; }
        [Display(Name = "Rank Full Name"), Required, StringLength(25)]
        public string Long { get; set; }
    }
}
