using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysSettingService
    {
        public Task<(int rowCount, IEnumerable<SystemIP> systemIPList)> GetSystemIPList(string sysID, string cultureID, int pageIndex, int pageSize)
        {
            return _sysSettingRepository.GetSystemIPList(sysID, cultureID, pageIndex, pageSize);
        }

        public Task EditSystemIPDetail(SystemIP systemIP, string userID)
        {
            systemIP.UpdUserID = userID;
            return _sysSettingRepository.EditSystemIPDetail(systemIP);
        }

        public Task DeleteSystemIPDetail(string sysID, string subSysID, string ipAddress)
        {
            return _sysSettingRepository.DeleteSystemIPDetail(sysID, subSysID, ipAddress);
        }
    }
}