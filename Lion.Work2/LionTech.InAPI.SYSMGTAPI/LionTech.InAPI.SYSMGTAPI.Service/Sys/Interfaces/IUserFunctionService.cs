using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface IUserFunctionService
    {
        Task<IEnumerable<UserFunction>> GetUserFunctions(string userID, string updUserID, string cultureID);
        Task EditUserFunction(UserFunctionDetail para);
        Task RecordUserFunction(string userID, string userNM, string erpWFNO, string memo);
        Task RecordUserFunApply(string userID, string userNM, string erpWFNO, string memo,
            List<UserFunctionValue> userFunctionList,
            IEnumerable<UserFunction> originUserFunctionList);
    }
}