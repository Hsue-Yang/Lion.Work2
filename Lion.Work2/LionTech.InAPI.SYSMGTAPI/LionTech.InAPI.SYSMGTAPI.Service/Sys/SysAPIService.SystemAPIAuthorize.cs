using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysAPIService
    {
        public Task<IEnumerable<SystemAPIAuthorize>> GetSystemAPIAuthorizeList(string sysID, string apiGroupID, string apiFunID, string cultureID)
        {
            return _sysAPIRepository.GetSystemAPIAuthorizeList(sysID, apiGroupID, apiFunID, cultureID);
        }

        public Task EditSystemAPIAuthorize(SystemAPIAuthorize systemAPIAuthorize)
        {
            return _sysAPIRepository.EditSystemAPIAuthorize(systemAPIAuthorize);
        }

        public Task DeleteSystemAPIAuthorize(string sysID, string apiGroupID, string apiFunID, string clientSysID)
        {
            return _sysAPIRepository.DeleteSystemAPIAuthorize(sysID, apiGroupID, apiFunID, clientSysID);
        }
    }
}
