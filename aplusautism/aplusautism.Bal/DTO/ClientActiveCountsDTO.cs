using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.DTO
{
    public class ClientActiveCountsDTO
    {
        public int? statusID { get; set; }

        public string StatusName { get; set; }

        public int? ClientCount { get; set; }


        public string TimePeriodForTime { get; set; }

        public decimal PaymentReceived { get; set; }
    }
}
