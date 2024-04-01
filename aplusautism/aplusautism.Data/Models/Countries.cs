using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Data.Models
{
    public class Countries
    {
        public long Id { get; set; }

        public long CountryId { get; set; }

        public string Name { get; set; }

        public string ISO2 { get; set; }

        public string ISO3 { get; set; }

        public string PhoneCode { get; set; }

        public string Currency { get; set; }

        public string CurrencyName { get; set; }
    }
}
