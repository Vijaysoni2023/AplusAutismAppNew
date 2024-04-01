using aplusautism.Bal.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.ILogic
{
    public interface IlessondetailsBycategory
    {

        List<LessonDetailsbycategoryDTO> GetlessonBycategory(string opcode, string UserLanguageId ,string LessonName, string userId, string userMainId, int UserStatus);

        List<PaymentsDTO> GetUserPaymentStatus(string opcode, string userId, string userMainId);
    }
}
