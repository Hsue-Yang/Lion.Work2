using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysFunToolSettingRepository
    {
        Task<IEnumerable<SystemFunToolSetting>> GetSystemFunToolSettings(string userID, string sysID, string funControllerID, string funActionName, string cultureID);
        Task<IEnumerable<SystemFunControllerID>> GetSystemFunToolControllerIDs(string sysID, string condition, string cultureID);
        Task<IEnumerable<SystemFunToolFunName>> GetSystemFunToolFunNames(string sysID, string funControllerID, string condition, string cultureID);
        Task EditSystemFunToolSetting(SystemUserToolPara systemUserToolPara);
        Task DeleteSystemFunToolSetting(string userID, string sysID, string funControllerID, string funActionName, SystemUserToolPara systemUserToolPara);
        Task CopySystemUserFunTool(string toolNO, SystemUserToolPara systemUserToolPara);
    }
}
