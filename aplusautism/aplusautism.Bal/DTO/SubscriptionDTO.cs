using aplusautism.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.DTO
{
    public class SubscriptionDTO: SubscriptionSetup
    {
        public bool? IsActive { get; set; }
        public string StartDateFormat { get; set; }
        public string EndDateFormat { get; set; }
    }
}
