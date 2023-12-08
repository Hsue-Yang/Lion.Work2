using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysRoleService
    {
        public Task<(int rowCount, IEnumerable<SystemRoleUser> systemRoleUserList)> GetSystemRoleUserList(string sysID, string roleID, string userID, string userNM, int pageIndex, int pageSize)
        {
            return _sysRoleRepository.GetSystemRoleUserList(sysID, roleID, userID, userNM, pageIndex, pageSize);
        }
    }
}
