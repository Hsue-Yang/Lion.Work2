using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysFunAssignService
    {
        Task<IEnumerable<SystemFunAssign>> GetSystemFunAssigns(string sysID, string funControllerID, string funActionName);
        Task EditSystemFunAssign(SystemFunAssignPara para);
        Task<IEnumerable<SysFunRawData>> GetFunRawDatas(List<UserFunctionValue> userFunction, string userID, string cultureID);
        Task<IEnumerable<UserMain>> GetUserRawDatas(List<UserMain> userIDList);
    }
}
