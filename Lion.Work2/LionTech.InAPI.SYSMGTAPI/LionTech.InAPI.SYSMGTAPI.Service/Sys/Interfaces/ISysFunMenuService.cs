using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysFunMenuService
    {
        Task<IEnumerable<SystemFunMenu>> GetSystemFunMenuByIdList(string sysID, string cultureID);

        Task<(int rowCount, IEnumerable<SystemFunMenu> SystemFunMenuList)> GetSystemFunMenuList(string sysID, string cultureID, int pageIndex, int pageSize);

        Task<SystemFunMenuMain> GetSystemFunMenuDetail(string sysID, string funMenu);

        Task EditSystemFunMenuDetail(SystemFunMenuMain systemFunMenu);

        Task<EnumDeleteSystemFunMenuResult> DeleteSystemFunMenuDetail(string sysID, string funMenu);
    }
}