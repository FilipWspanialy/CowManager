using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CowManager.Models.Models;
using Microsoft.AspNetCore.Identity;

namespace CowManager.Models.Models
{
    public class Customer
    {
        public int? Id { get; set; }


        public string? Name { get; set; }


        public string? UserId { get; set; }


        public IdentityUser User { get; set; }
    }

}