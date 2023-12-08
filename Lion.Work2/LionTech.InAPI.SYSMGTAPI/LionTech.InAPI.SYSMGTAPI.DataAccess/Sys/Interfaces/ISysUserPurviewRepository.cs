using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysUserPurviewRepository
    {
        Task<IEnumerable<SysUserPurview>> GetSysUserPurviews(string userID, string updUserID, string cultureID);
        Task<IEnumerable<PurviewName>> GetPurviewNames(string sysID, string cultureID);
        Task<IEnumerable<SysUserPurviewDetails>> GetSysUserPurviewDetails(string sysID, string userID, string cultureID);
        Task EditSysUserPurviewDetail(UserPurviewPara userPurviewPara);
    }
}
