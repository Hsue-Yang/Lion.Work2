using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysFunElmRepository
    {
        Task<(int rowCount, IEnumerable<SystemFunElm> systemFunElms)> GetSysFunElmList(string sysID, string isDisable, string elmID, string elmName, string funControllerID, string funActionName, string cultureID, int pageIndex, int pageSize);
        Task<bool> CheckSystemFunElmIdIsExists(string sysID, string elmID, string funControllerID, string funActionName);
        Task<SystemFunElm> GetSystemFunElmDetail(string sysID, string elmID, string funControllerID, string funActionName);
        Task<IEnumerable<SystemRoleFunElm>> GetSystemFunElmRoleList(string sysID, string elmID, string funControllerID, string funActionName, string cultureID);
        Task<SystemFunElm> GetSystemFunElmInfo(string sysID, string elmID, string funControllerID, string funActionName, string cultureID);
        Task EditSystemFunElmDetail(SystemFunElm systemFunElm);
        Task EditSystemFunElmRole(string sysID, string elmID, string funControllerID, string funActionName, List<ElmRoleInfoValue> elmRoleInfoValues);
    }
}