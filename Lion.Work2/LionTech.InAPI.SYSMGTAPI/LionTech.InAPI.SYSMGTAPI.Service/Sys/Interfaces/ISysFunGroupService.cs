using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysFunGroupService
    {
        Task<IEnumerable<SystemFunGroup>> GetUserSystemFunGroupList(string userID, string cultureID);

        Task<IEnumerable<SystemFunGroup>> GetSystemFunGroupByIdList(string sysID, string cultureID);

        Task<(int rowCount, IEnumerable<SystemFunGroup> SystemFunGroupList)> GetSystemFunGroupList(string sysID, string cultureID, int pageIndex, int pageSize);

        Task<SystemFunGroupMain> GetSystemFunGroupDetail(string sysID, string funControllerID);

        Task EditSystemFunGroupDetail(SystemFunGroupMain systemFunGroup);

        Task<EnumDeleteSystemFunGroupDetailResult> DeleteSystemFunGroupDetail(string sysID, string funControllerID, string userID, string execSysID, string execIpAddress);
    }
}