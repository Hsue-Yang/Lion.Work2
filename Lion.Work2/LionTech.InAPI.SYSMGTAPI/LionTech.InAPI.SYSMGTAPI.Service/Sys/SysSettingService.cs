using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysSettingService : ISysSettingService
    {
        private readonly ISysSettingRepository _sysSettingRepository;

        public SysSettingService(ISysSettingRepository sysSettingRepository)
        {
            _sysSettingRepository = sysSettingRepository;
        }

        public Task<bool> CheckIsITManager(string sysID, string userID)
        {
            return _sysSettingRepository.CheckIsITManager(sysID, userID);
        }

        public async Task<IEnumerable<CMCode>> GetCodeManagementList(string codeKind, string cultureID, List<string> codeIDs, string codeParent, bool isFormatNMID)
        {
            List<CMCode> codeIDParas = codeIDs.Select(c => new CMCode() { CodeID = c }).ToList();

            var result = await _sysSettingRepository.GetCodeManagementList(codeKind, cultureID, codeIDParas, codeParent);

            return isFormatNMID ? result.Select(c => { c.CodeNM = string.Format("{0} ({1})", c.CodeNM, c.CodeID); return c; }) : result;
        }

        public Task<string> GetSystemFilePath(string sysID)
        {
            return _sysSettingRepository.GetSystemFilePath(sysID);
        }

        public Task<IEnumerable<SystemSetting>> GetAllSystemByIdList(string cultureID)
        {
            return _sysSettingRepository.GetAllSystemByIdList(cultureID);
        }

        public Task<IEnumerable<SystemSetting>> GetUserSystemByIdList(string userID, bool isExcludeOutsourcing, string cultureID)
        {
            return _sysSettingRepository.GetUserSystemByIdList(userID, isExcludeOutsourcing, cultureID);
        }

        public Task<List<SystemSetting>> GetSystemSettingList(string userID, string cultureID)
        {
            return _sysSettingRepository.GetSystemSettingList(userID, cultureID);
        }

        public Task<SystemMain> GetSystemMain(string sysID)
        {
            return _sysSettingRepository.GetSystemMain(sysID);
        }

        public Task<bool> EditSystemSettingDetail(SystemMain systemMain, string userID, string clientIPAddress)
        {
            return _sysSettingRepository.EditSystemSettingDetail(systemMain, userID, clientIPAddress);
        }

        public Task<EnumDeleteSystemSettingResult> DeleteSystemSettingDetail(string sysID)
        {
            return _sysSettingRepository.DeleteSystemSettingDetail(sysID);
        }

        public Task<IEnumerable<SystemSysID>> GetUserSystemSysIDs(string userID, bool excludeOutsourcing, string cultureID)
        {
            return _sysSettingRepository.GetUserSystemSysIDs(userID, excludeOutsourcing, cultureID);
        }

        public Task<IEnumerable<SystemSysID>> GetSystemSysIDs(bool excludeOutsourcing, string cultureID)
        {
            return _sysSettingRepository.GetSystemSysIDs(excludeOutsourcing, cultureID);
        }

        public Task<IEnumerable<SystemRoleGroup>> GetSystemRoleGroups(string cultureID)
        {
            return _sysSettingRepository.GetSystemRoleGroups(cultureID);

        }

        public Task<IEnumerable<SystemConditionID>> GetSystemConditionIDs(string sysID, string cultureID)
        {
            return _sysSettingRepository.GetSystemConditionIDs(sysID, cultureID);
        }
    }
}