using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface IUserFunctionRepository
    {
        Task<IEnumerable<UserFunction>> GetUserFunctions(string userID, string updUserID, string cultureID);
        Task EditUserFunction(string userID, string IsDisable, string updUserID, List<UserFunctionValue> userFunctionValueList);
        Task<IEnumerable<UserFunction>> GetAllSystemUserFunctions(string userID, string cultureID);
    }
}