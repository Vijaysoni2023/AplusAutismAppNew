
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Data.Models
{
    public class AppUser : IdentityUser
    {
        public string? Userrole { get; set; }

        [StringLength(150)]
        public string? firstName { get; set; }

        [StringLength(150)]
        public string? lastName { get; set; }

        [StringLength(12)]
        public string? phone { get; set; }


        public string? trackingRegisterId { get; set; }
    }
}
