using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CowManager.Models.Models
{
    public class UserDetailsViewModel
    {
        public IdentityUser User { get; set; }
        public List<Cow> Cows { get; set; }
        public List<Diagnosis> Diagnoses { get; set; }
        public string CurrentUserId { get; set; }
    }
}
