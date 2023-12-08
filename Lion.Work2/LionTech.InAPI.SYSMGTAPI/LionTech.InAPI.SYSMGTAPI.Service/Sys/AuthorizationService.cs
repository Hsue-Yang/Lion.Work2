using System.Linq;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationRepository _authorizationRepository;

        public AuthorizationService(IAuthorizationRepository authorizationRepository)
        {
            _authorizationRepository = authorizationRepository;
        }

        public async Task<object> GetAllUserSystemRolesObj()
        {
            var userRoleList = await _authorizationRepository.GetAllUserSystemRoles();
            return userRoleList
                .GroupBy(userSysRole => new { userSysRole.UserID })
                .Select(g => new
                {
                    g.Key.UserID,
                    RoleList = (from s in g
                                select new
                                {
                                    s.SysID,
                                    s.SysNM,
                                    s.RoleID,
                                    s.RoleNM
                                })
                }).ToList();
        }

        public async Task<object> GetAllUserAssignSystemFunsObj()
        {
            var userFunList = await _authorizationRepository.GetAllUserAssignSystemFuns();
            return userFunList
                .GroupBy(userSysFun => new { userSysFun.UserID })
                .Select(g => new
                {
                    g.Key.UserID,
                    FunList = from s in g
                              select new
                              {
                                  s.SysID,
                                  s.SysNM,
                                  s.ControllerID,
                                  s.ControllerNM,
                                  s.ActionName,
                                  s.ActionNM
                              }
                }).ToList();
        }
    }
}