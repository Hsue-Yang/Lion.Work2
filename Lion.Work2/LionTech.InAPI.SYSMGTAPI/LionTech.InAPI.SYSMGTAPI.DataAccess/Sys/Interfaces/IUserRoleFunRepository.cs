using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface IUserRoleFunRepository
    {
        Task<(int rowCount, IEnumerable<UserRoleFun> userRoleFunList)> GetUserRoleFuns(string userID, string userNM, int pageIndex, int pageSize);
        Task<UserMain> GetUserMainInfo(string userID);
        Task<IEnumerable<SystemRoleGroupCollect>> GetSystemRoleGroupCollects(string roleGroupID);
        Task<IEnumerable<UserSystemRoleData>> GetUserSystemRoles(string userID, string updUserID, string cultureID);
        Task EditUserSystemRole(UserRoleFunDetailPara userRoleFunDetailPara, List<SystemRoleMain> userRoleFunDetailParaList);
        Task<IEnumerable<UserMenuFun>> GetUserMenuFuns(string userID, string cultureID);
    }
}
