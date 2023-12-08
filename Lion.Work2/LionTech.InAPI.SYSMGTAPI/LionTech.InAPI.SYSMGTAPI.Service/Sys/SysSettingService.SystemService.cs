using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysSettingService
    {
        public Task<IEnumerable<SystemService>> GetSystemServiceList(string sysID, string cultureID)
        {
            return _sysSettingRepository.GetSystemServiceList(sysID, cultureID);
        }

        public Task EditSystemServiceDetail(SystemService systemService, string userID)
        {
            systemService.UpdUserID = userID;
            return _sysSettingRepository.EditSystemServiceDetail(systemService);
        }

        public Task DeleteSystemServiceDetail(string sysID, string subSysID, string serviceID)
        {
            return _sysSettingRepository.DeleteSystemServiceDetail(sysID, subSysID, serviceID);
        }
    }
}