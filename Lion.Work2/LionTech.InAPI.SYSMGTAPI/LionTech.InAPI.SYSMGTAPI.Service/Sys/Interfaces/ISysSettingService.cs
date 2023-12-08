using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysSettingService
    {
        Task<bool> CheckIsITManager(string sysID, string userID);

        Task<IEnumerable<CMCode>> GetCodeManagementList(string codeKind, string cultureID, List<string> codeIDs, string codeParent, bool isFormatNMID);

        Task<string> GetSystemFilePath(string sysID);

        Task<IEnumerable<SystemSetting>> GetAllSystemByIdList(string cultureID);

        Task<IEnumerable<SystemSetting>> GetUserSystemByIdList(string userID, bool isExcludeOutsourcing, string cultureID);

        Task<List<SystemSetting>> GetSystemSettingList(string userID, string cultureID);

        Task<SystemMain> GetSystemMain(string sysID);

        Task<bool> EditSystemSettingDetail(SystemMain systemMain, string userID, string clientIPAddress);

        Task<EnumDeleteSystemSettingResult> DeleteSystemSettingDetail(string sysID);

        Task<(int rowCount, IEnumerable<SystemIP> systemIPList)> GetSystemIPList(string sysID, string cultureID, int pageIndex, int pageSize);

        Task EditSystemIPDetail(SystemIP systemIP, string userID);

        Task DeleteSystemIPDetail(string sysID, string subSysID, string ipAddress);

        Task<IEnumerable<SystemService>> GetSystemServiceList(string sysID, string cultureID);

        Task EditSystemServiceDetail(SystemService systemService, string userID);

        Task DeleteSystemServiceDetail(string sysID, string subSysID, string serviceID);

        Task<IEnumerable<SystemSub>> GetUserSystemSubList(string userID, string cultureID);

        Task<IEnumerable<SystemSetting.SystemSub>> GetSystemSubByIdList(string sysID, string cultureID);

        Task<IEnumerable<SystemSub>> GetSystemSubList(string sysID);

        Task EditSystemSubDetail(SystemSub systemSub, string userID);

        Task DeleteSystemSubDetail(string sysID);

        Task<(int rowCount, IEnumerable<SystemUser> systemUserList)> GetSystemUserList(string sysID, string userID, string userNM, int pageIndex, int pageSize);

        Task<IEnumerable<SystemSysID>> GetUserSystemSysIDs(string userID, bool excludeOutsourcing, string cultureID);

        Task<IEnumerable<SystemSysID>> GetSystemSysIDs(bool excludeOutsourcing, string cultureID);

        Task<IEnumerable<SystemRoleGroup>> GetSystemRoleGroups(string cultureID);

        Task<IEnumerable<SystemConditionID>> GetSystemConditionIDs(string sysID, string cultureID);
    }
}