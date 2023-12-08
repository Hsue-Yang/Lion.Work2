using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysRoleCategoryService
    {
        Task<IEnumerable<SystemRoleCategory>> GetSystemRoleCategoryByIdList(string sysID, string cultureID);

        Task<IEnumerable<SystemRoleCategory>> GetSystemRoleCategoryList(string sysID, string roleCategoryNM, string cultureID);

        Task<SystemRoleCategoryMain> GetSystemRoleCategoryDetail(string sysID, string roleCategoryID);

        Task EditSystemRoleCategoryDetail(SystemRoleCategoryMain systemRoleCategory);

        Task<EnumDeleteSystemRoleCategoryDetailResult> DeleteSystemRoleCategoryDetail(string sysID, string roleCategoryID);
    }
}
