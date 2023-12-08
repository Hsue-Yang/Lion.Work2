using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysRoleService
    {

        public Task<(int rowCount, IEnumerable<SystemRoleElms> systemRoleElmList)> GetSystemRoleElmsList(string sysID, string roleID, string cultureID, string funControllerId, string funactionNM, int pageIndex, int pageSize)
        {
            return _sysRoleRepository.GetSystemRoleElmsList(sysID, roleID, cultureID, funControllerId, funactionNM, pageIndex, pageSize);
        }

        public Task EditSystemRoleElmsList(SystemRoleElmEditLists systemRoleElmEditLists)
        {
            return _sysRoleRepository.EditSystemRoleElmsList(systemRoleElmEditLists);
        }

        public Task<IEnumerable<SysElmName>> GetSystemFunElmByIdList(string sysID, string cultureID, string funControllerId, string funactionNM)
        {
            return _sysRoleRepository.GetSystemFunElmByIdList(sysID, cultureID,  funControllerId, funactionNM);
        }
    }
}