using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class UserPermissionService : IUserPermissionService
    {
        private readonly IUserPermissionRepository _userPermissionRepository;

        public UserPermissionService(IUserPermissionRepository userPermissionRepository)
        {
            _userPermissionRepository = userPermissionRepository;
        }

        public async Task<(int rowCount, IEnumerable<UserPermission> userPermissionsList)> GetUserPermissionList(string userID, string userNM, string restrictType, string cultureID, int pageIndex, int pageSize)
        {
            return await _userPermissionRepository.GetUserPermissionList(userID, userNM, restrictType, cultureID, pageIndex, pageSize);
        }

        public async Task<UserPermissionDetail> GetUserPermissionDetail(string userID)
        {
            return await _userPermissionRepository.GetUserPermissionDetail(userID);
        }
    }
}
