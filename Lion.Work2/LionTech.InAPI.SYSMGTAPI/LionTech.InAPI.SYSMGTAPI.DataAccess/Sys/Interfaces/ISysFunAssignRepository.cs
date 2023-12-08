using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysFunAssignRepository
    {
        Task<IEnumerable<SystemFunAssign>> GetSystemFunAssigns(string sysID, string funControllerID, string funActionName);
        Task EditSystemFunAssign(string sysID, string funControllerID, string funActionName, string updUserID, List<SystemFunAssignUser> userIDList);
        Task<IEnumerable<SysFunRawData>> GetFunRawDatas(List<UserFunctionValue> userFunction, string userID, string cultureID);
        Task<IEnumerable<UserMain>> GetUserRawDatas(List<UserMain> userIDList);
    }
}
