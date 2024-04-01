using aplusautism.Bal.DTO;
using aplusautism.Data.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.ILogic
{
    public interface ISubscriptionsetup
    {


        List<SubscriptionSetup> GetSubscriptionData();

        SubscriptionSetup EditSubscriptionData(int subsid);


        SubscriptionSetup UpdateSubscriptionData(UpdateSubscriptionPlanDTO updatedata);

        List<SubscriptionDTO> GetLatestSubscriptionData(string opcode);

        UserDTO SaveDeviceTracking(DeviceTrackingDTO deviceTrackingDTO);

    }
}
