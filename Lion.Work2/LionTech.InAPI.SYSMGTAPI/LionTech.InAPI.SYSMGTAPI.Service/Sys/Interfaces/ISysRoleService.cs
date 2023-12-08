using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysRoleService
    {
        Task<IEnumerable<SystemRole>> GetSystemRoleByIdList(string sysID, string roleCategoryID, string cultureID);

        Task<(int rowCount, IEnumerable<SystemRole> SystemRoleList)> GetSystemRoleList(string sysID, string roleID, string roleCategoryID, string cultureID, int pageIndex, int pageSize);

        Task<SystemRoleMain> GetSystemRole(string sysID, string roleID);

        Task EditSystemRoleByCategory(SystemRoleMain systemRole);

        Task EditSystemRoleDetail(SystemRoleMain systemRole);

        Task DeleteSystemRoleDetail(string sysID, string roleID);

        Task<(int rowCount, IEnumerable<SystemRoleUser> systemRoleUserList)> GetSystemRoleUserList(string sysID, string roleID, string userID, string userNM, int pageIndex, int pageSize);

        Task EditSystemRoleFunList(SystemRoleFunEditLists systemRoleFunEditLists);

        Task<(int rowCount, IEnumerable<SystemRoleFunList> systemRoleFunList)> GetSystemRoleFunList(string sysID, string roleID, string funControllerId, string cultureID, int pageIndex, int pageSize);
        
        Task<(int rowCount, IEnumerable<SystemRoleElms> systemRoleElmList)> GetSystemRoleElmsList(string sysID, string roleID, string cultureID, string funControllerID, string funactionNM, int pageIndex, int pageSize);
        
        Task EditSystemRoleElmsList(SystemRoleElmEditLists systemRoleElmEditLists);
        
        Task<IEnumerable<SysElmName>> GetSystemFunElmByIdList(string sysID, string cultureID, string funControllerID, string funactionNM);

    }
}