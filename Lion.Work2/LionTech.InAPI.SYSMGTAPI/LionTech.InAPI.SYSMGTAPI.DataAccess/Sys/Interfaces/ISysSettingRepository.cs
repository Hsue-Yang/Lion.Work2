using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysSettingRepository
    {
        Task<bool> CheckIsITManager(string sysID, string userID);

        Task<IEnumerable<CMCode>> GetCodeManagementList(string codeKind, string cultureID, List<CMCode> codeIDParas, string codeParent);

        Task<string> GetSystemFilePath(string sysID);

        Task<IEnumerable<SystemSetting>> GetAllSystemByIdList(string cultureID);

        Task<IEnumerable<SystemSetting>> GetUserSystemByIdList(string userID, bool isExcludeOutsourcing, string cultureID);

        #region - SystemSetting -
        Task<List<SystemSetting>> GetSystemSettingList(string userID, string cultureID);

        Task<SystemMain> GetSystemMain(string sysID);

        Task<bool> EditSystemSettingDetail(SystemMain systemMain, string userID, string clientIPAddress);

        Task<EnumDeleteSystemSettingResult> DeleteSystemSettingDetail(string sysID);

        Task<IEnumerable<SystemSysID>> GetUserSystemSysIDs(string userID, bool excludeOutsourcing, string cultureID);

        Task<IEnumerable<SystemSysID>> GetSystemSysIDs(bool excludeOutsourcing, string cultureID);

        Task<IEnumerable<SystemRoleGroup>> GetSystemRoleGroups(string cultureID);

        Task<IEnumerable<SystemConditionID>> GetSystemConditionIDs(string sysID, string cultureID);
        #endregion

        #region - SystemIP -
        Task<(int rowCount, IEnumerable<SystemIP> systemIPList)> GetSystemIPList(string sysID, string cultureID, int pageIndex, int pageSize);

        Task EditSystemIPDetail(SystemIP systemIP);

        Task DeleteSystemIPDetail(string sysID, string subSysID, string ipAddress);
        #endregion

        #region - SystemService -
        Task<IEnumerable<SystemService>> GetSystemServiceList(string sysID, string cultureID);

        Task EditSystemServiceDetail(SystemService systemService);

        Task DeleteSystemServiceDetail(string sysID, string subSysID, string serviceID);
        #endregion

        #region - SystemSub -
        Task<IEnumerable<SystemSub>> GetUserSystemSubList(string userID, string cultureID);

        Task<IEnumerable<SystemSetting.SystemSub>> GetSystemSubByIdList(string sysID, string cultureID);

        Task<IEnumerable<SystemSub>> GetSystemSubList(string sysID);

        Task EditSystemSubDetail(SystemSub systemSub);

        Task DeleteSystemSubDetail(string sysID);
        #endregion

        #region - SystemUser -
        Task<(int rowCount, IEnumerable<SystemUser> systemUserList)> GetSystemUserList(string sysID, string userID, string userNM, int pageIndex, int pageSize);
        #endregion
    }
}