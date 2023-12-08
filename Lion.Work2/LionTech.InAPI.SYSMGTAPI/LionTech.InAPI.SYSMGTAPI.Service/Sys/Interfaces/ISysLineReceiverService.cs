using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysLineReceiverService
    {
        Task<bool> CheckSystemLineBotReceiverIdIsExists(string sysID, string lineID, string lineReceiverID, string receiverID);
        Task<(int rowCount, IEnumerable<SystemLineReceiver> systemLineReceiverList)> GetSystemLineBotReceiver(string sysID, string lineID, string cultureID,string queryReceiverNM, int pageIndex, int pageSize);
        Task<SystemLineReceiver> GetSystemLineBotReceiverDetail(string sysID, string receiverID, string lineID, string cultureID);
        Task EditSystemLineBotReceiverDetail(SystemLineReceiver systemLineReceiver);
    }
}
