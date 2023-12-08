using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysFunService
    {
        Task<IEnumerable<SystemFun>> GetUserSystemFunList(string userID, string cultureID);
        
        Task<(int rowCount, IEnumerable<SystemFun> SystemFunList)> GetSystemFunList(string sysID, string subSysID, string funControllerID, string funActionName, string funGroupNM, string funNM, string funMenuSysID, string funMenu, string cultureID, int pageIndex, int pageSize);

        Task<SystemFunMain> GetSystemFunDetail(string sysID, string funControllerID, string funActionName, string cultureID);

        Task<IEnumerable<SystemRoleFun>> GetSystemFunRoleList(string sysID, string funControllerID, string funActionName, string cultureID);

        Task EditSystemFunByPurview(SystemFunMain systemFun);

        Task EditSystemFunDetail(DataTable systemFun, DataTable systemFunRoles, DataTable systemMenuFuns);

        Task<EnumDeleteSystemFunDetailResult> DeleteSystemFunDetail(string sysID, string funControllerID, string funActionName);

        Task<IEnumerable<SystemMenuFun>> GetSystemMenuFunList(string sysID, string funControllerID, string funActionName);

        Task<IEnumerable<SystemFunToolFunName>> GetSystemFunNameList(string sysID, string funControllerID, string cultureID);

        Task<IEnumerable<SystemFunAction>> GetSystemFunActionList(string cultureID);
    }
}