using aplusautism.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.DTO
{
    public class RegisterUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string ContactNumber { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Languages { get; set; }
        public List<GlobalCodes> LanguagesList { get; set; }

        public List<Countries> Countrieslist { get; set; }

        public List<States> StatesList { get; set; }

        public List<Cities> citiesList { get; set; }

        public bool checkTerm { get; set; }
        
    }
}
