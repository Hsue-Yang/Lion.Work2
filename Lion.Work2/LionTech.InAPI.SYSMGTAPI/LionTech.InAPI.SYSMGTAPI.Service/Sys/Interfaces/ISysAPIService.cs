using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysAPIService
    {
        Task<SystemAPI> GetSystemAPIFullName(string sysID, string apiGroupID, string apiFunID, string cultureID);

        Task<IEnumerable<SystemAPI>> GetSystemAPIByIdList(string sysID, string apiGroup, string cultureID);

        Task<(int rowCount, IEnumerable<SystemAPI> SystemAPIList)> GetSystemAPIList(string sysID, string apiGroupID, string cultureID, int pageIndex, int pageSize);

        Task<SystemAPIMain> GetSystemAPIDetail(string sysID, string apiGroupID, string apiFunID);

        Task<IEnumerable<SystemAPIRole>> GetSystemAPIRoleList(string sysID, string apiGroupID, string apiFunID, string cultureID);

        Task EditSystemAPIDetail(SystemAPIMain systemAPI, DataTable systemRoleAPIs);

        Task<EnumDeleteSystemAPIDetailResult> DeleteSystemAPIDetail(string sysID, string apiGroupID, string apiFunID);

        Task<IEnumerable<SystemAPIAuthorize>> GetSystemAPIAuthorizeList(string sysID, string apiGroupID, string apiFunID, string cultureID);

        Task EditSystemAPIAuthorize(SystemAPIAuthorize systemAPIAuthorize);

        Task DeleteSystemAPIAuthorize(string sysID, string apiGroupID, string apiFunID, string clientSysID);

        Task<(int rowCount, IEnumerable<SystemAPIClient> SystemAPILogList)> GetSystemAPILogList(string sysID, string apiGroupID, string apiFunID, string apiClientSysID, string apiNo, string dtBegin, string dtEnd, string cultureID, int pageIndex, int pageSize);

        Task<(int rowCount, IEnumerable<SystemAPIClient> SystemAPIClientList)> GetSystemAPIClientList(string sysID, string apiGroupID, string apiFunID, string dtBegin, string dtEnd, string cultureID, int pageIndex, int pageSize);

        Task<SystemAPIClient> GetSystemAPIClientDetail(string apiNo, string cultureID);

        Task<IEnumerable<SystemAPIFuntion>> GetSystemAPIFuntionList(string sysID, string apiControllerID, string cultureID);
    }
}
