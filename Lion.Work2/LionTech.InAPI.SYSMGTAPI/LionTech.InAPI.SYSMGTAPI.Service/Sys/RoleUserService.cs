using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class RoleUserService : IRoleUserService
    {
        private readonly IRoleUserRepository _roleUserRepository;

        public RoleUserService(IRoleUserRepository roleUserRepository)
        {
            _roleUserRepository = roleUserRepository;
        }
        public async Task<IEnumerable<RoleUser>> GetRoleUsers(string sysID, string roleID, string cultureID)
        {
            return await _roleUserRepository.GetRoleUsers(sysID, roleID, cultureID);
        }
    }
}
