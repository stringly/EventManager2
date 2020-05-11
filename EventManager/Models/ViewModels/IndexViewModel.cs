using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Models.ViewModels
{
    public abstract class IndexViewModel
    {
        public string CurrentSort { get; set; }
        public string CurrentFilter { get; set; }
        public PagingInfo PagingInfo { get; set;}
    }
}
