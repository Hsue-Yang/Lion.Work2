using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysLineReceiverService : ISysLineReceiverService
    {
        private readonly ISysLineReceiverRepository _sysLineReceiverRepository;

        public SysLineReceiverService(ISysLineReceiverRepository sysLineReceiverRepository)
        {
            _sysLineReceiverRepository = sysLineReceiverRepository;
        }
        public Task<bool> CheckSystemLineBotReceiverIdIsExists(string sysID, string lineID, string lineReceiverID, string receiverID)
        {
            return _sysLineReceiverRepository.CheckSystemLineBotReceiverIdIsExists(sysID, lineID, lineReceiverID, receiverID);
        }

        public Task<(int rowCount, IEnumerable<SystemLineReceiver> systemLineReceiverList)> GetSystemLineBotReceiver(string sysID, string lineID, string cultureID, string queryReceiverNM, int pageIndex, int pageSize)
        {
            return _sysLineReceiverRepository.GetSystemLineBotReceiver(sysID, lineID, cultureID, queryReceiverNM, pageIndex, pageSize);
        }

        public Task<SystemLineReceiver> GetSystemLineBotReceiverDetail(string sysID, string receiverID, string lineID, string cultureID)
        {
            return _sysLineReceiverRepository.GetSystemLineBotReceiverDetail(sysID, receiverID, lineID, cultureID);
        }

        public Task EditSystemLineBotReceiverDetail(SystemLineReceiver systemLineReceiver)
        {
            return _sysLineReceiverRepository.EditSystemLineBotReceiverDetail(systemLineReceiver);
        } 
    }
}
