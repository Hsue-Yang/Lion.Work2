using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysSettingService
    {
        public Task<(int rowCount, IEnumerable<SystemUser> systemUserList)> GetSystemUserList(string sysID, string userID, string userNM, int pageIndex, int pageSize)
        {
            return _sysSettingRepository.GetSystemUserList(sysID, userID, userNM, pageIndex, pageSize);
        }
    }
}