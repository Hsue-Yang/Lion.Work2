using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysUserPurviewService
    {
        Task<IEnumerable<SysUserPurview>> GetSysUserPurviews(string userID, string updUserID, string cultureID);
        Task<IEnumerable<PurviewName>> GetPurviewNames(string sysID, string cultureID);
        Task<IEnumerable<SysUserPurviewDetails>> GetSysUserPurviewDetails(string sysID, string userID, string cultureID);
        Task EditSysUserPurviewDetail(UserPurviewPara userPurviewPara);
    }
}
