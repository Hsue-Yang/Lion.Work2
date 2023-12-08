using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysRoleService
    {
        public Task<(int rowCount, IEnumerable<SystemRoleFunList> systemRoleFunList)> GetSystemRoleFunList(string sysID, string roleID, string funControllerId, string cultureID, int pageIndex, int pageSize)
        {
            return _sysRoleRepository.GetSystemRoleFunList(sysID, roleID, funControllerId, cultureID, pageIndex, pageSize);
        }

        public Task EditSystemRoleFunList(SystemRoleFunEditLists systemRoleFunEditLists)
        {
            return _sysRoleRepository.EditSystemRoleFunList(systemRoleFunEditLists);
        }
    }
}