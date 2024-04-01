using aplusautism.Bal.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.ILogic
{
    public interface Iadmindashboard
    {
        List<ClientActiveCountsDTO> getclientstatusfordashboard(string opcode);

    }
}
