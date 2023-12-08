using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysRoleConditionService
    {
        Task<(int rowCount, IEnumerable<SystemRoleCondition>)> GetSystemRoleConditions(string roleConditionID, string roleID, string sysID, string cultureID, int pageIndex, int pageSize);
        Task<SystemRoleConditionDetail> GetSystemRoleConditionDetail(string sysID, string roleConditionID);
        Task EditSystemRoleConditionDetail(SysRoleConditionDetailPara sysRoleConditionDetailPara);
        Task DeleteSystemRoleConditionDetail(string sysID, string roleConditionID);
    }
}
