using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysFunToolSettingService : ISysFunToolSettingService
    {
        private readonly ISysFunToolSettingRepository _sysFunToolSettingRepository;

        public SysFunToolSettingService(ISysFunToolSettingRepository sysFunToolSettingRepository)
        {
            _sysFunToolSettingRepository = sysFunToolSettingRepository;
        }

        public Task<IEnumerable<SystemFunToolSetting>> GetSystemFunToolSettings(string userID, string sysID, string funControllerID, string funActionName, string cultureID)
        {
            return _sysFunToolSettingRepository.GetSystemFunToolSettings(userID, sysID, funControllerID, funActionName, cultureID);
        }

        public Task<IEnumerable<SystemFunControllerID>> GetSystemFunToolControllerIDs(string sysID, string condition, string cultureID)
        {
            return _sysFunToolSettingRepository.GetSystemFunToolControllerIDs(sysID, condition, cultureID);
        }

        public Task<IEnumerable<SystemFunToolFunName>> GetSystemFunToolFunNames(string sysID, string funControllerID, string condition, string cultureID)
        {
            return _sysFunToolSettingRepository.GetSystemFunToolFunNames(sysID, funControllerID, condition, cultureID);
        }

        public Task EditSystemFunToolSetting(SystemUserToolPara systemFunToolSettingPara)
        {
            return _sysFunToolSettingRepository.EditSystemFunToolSetting(systemFunToolSettingPara);
        }

        public Task DeleteSystemFunToolSetting(string userID, string sysID, string funControllerID, string funActionName, SystemUserToolPara systemFunToolSettingPara)
        {
            return _sysFunToolSettingRepository.DeleteSystemFunToolSetting(userID, sysID, funControllerID, funActionName, systemFunToolSettingPara);
        }
        public Task CopySystemUserFunTool(string toolNO, SystemUserToolPara systemFunToolSettingPara)
        {
            return _sysFunToolSettingRepository.CopySystemUserFunTool(toolNO, systemFunToolSettingPara);
        }
    }
}
