using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface IAuthorizationRepository
    {
        Task<IEnumerable<UserSysRole>> GetAllUserSystemRoles();

        Task<IEnumerable<UserSysFun>> GetAllUserAssignSystemFuns();
    }
}