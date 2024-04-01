using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.DTO
{
    public class AdmindashboardViewmodelDTO
    {
        public List<ClientActiveCountsDTO> ClientStatusList { get; set; }

        public List<ClientActiveCountsDTO> DashBoardpayment { get; set; }
    }
}
