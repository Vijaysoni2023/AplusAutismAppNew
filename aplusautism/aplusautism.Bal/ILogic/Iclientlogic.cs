using aplusautism.Bal.DTO;
using aplusautism.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.ILogic
{
    public interface Iclientlogic
    {
        List<ABuserDTO> GetClientList(string opcode, string pOpParams);

        string DeleteClient(int ClientId );
        List<ABuserDTO> GetClientDetail(int UserId);
    }
}
