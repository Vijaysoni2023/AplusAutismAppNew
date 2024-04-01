using aplusautism.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.DTO
{
    public class ABuserDTO
    {
        public int? AB_UserID { get; set; }



        public int? AB_MainID { get; set; }
        public string AB_Type { get; set; }
        public string UserType { get; set; }
         public string Status { get; set; }

        public string StatusName { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string FormatedName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public string StartDateFormat { get; set; }
        public string EndDateFormat { get; set; }

        public decimal TotalAmount { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }

        public string CityName { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<GlobalCodes> Statuslist { get; set; }

        public string oldstatus { get; set; }

        public string PreferedLanguage { get; set; }


    }
}
