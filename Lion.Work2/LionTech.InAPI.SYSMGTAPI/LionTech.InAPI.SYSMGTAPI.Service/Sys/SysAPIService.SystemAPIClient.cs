using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysAPIService
    {
        public Task<(int rowCount, IEnumerable<SystemAPIClient> SystemAPILogList)> GetSystemAPILogList(string sysID, string apiGroupID, string apiFunID, string apiClientSysID, string apiNo, string dtBegin, string dtEnd, string cultureID, int pageIndex, int pageSize)
        {
            return _sysAPIRepository.GetSystemAPILogList(sysID, apiGroupID, apiFunID, apiClientSysID, apiNo, dtBegin, dtEnd, cultureID, pageIndex, pageSize);
        }

        public Task<(int rowCount, IEnumerable<SystemAPIClient> SystemAPIClientList)> GetSystemAPIClientList(string sysID, string apiGroupID, string apiFunID, string dtBegin, string dtEnd, string cultureID, int pageIndex, int pageSize)
        {
            return _sysAPIRepository.GetSystemAPIClientList(sysID, apiGroupID, apiFunID, dtBegin, dtEnd, cultureID, pageIndex, pageSize);
        }

        public Task<SystemAPIClient> GetSystemAPIClientDetail(string apiNo, string cultureID)
        {
            return _sysAPIRepository.GetSystemAPIClientDetail(apiNo, cultureID);
        }
    }
}
