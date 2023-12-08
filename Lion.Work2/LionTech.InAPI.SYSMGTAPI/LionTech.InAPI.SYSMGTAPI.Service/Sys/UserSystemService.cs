using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class UserSystemService : IUserSystemService
    {
        private readonly IUserSystemRepository _userSystemRepository;

        public UserSystemService(IUserSystemRepository userSystemRepository)
        {
            _userSystemRepository = userSystemRepository;
        }

        public async Task<(int rowCount, IEnumerable<UserSystem> UserSystemList)> GetUserStatusList(string userID, string userNM, int pageIndex, int pageSize)
        {
            return await _userSystemRepository.GetUserStatusList( userID, userNM, pageIndex, pageSize);
        }

        public async Task<UserSystem> GetUserRawData(string userID)
        {
            return await _userSystemRepository.GetUserRawData(userID);
        }

        public async Task<IEnumerable<UserSystemRole>> GetUserOutSourcingSystemRoles(string userID, string cultureID)
        {
            return await _userSystemRepository.GetUserOutSourcingSystemRoles(userID, cultureID);
        }
        
        public async Task EditUserOutSourcingSystemRoles(UserSystemRoleParas para)
        {
            List<UserSystemRolePara> sysIDList = para.UserSystemRoleList.Select(x => new UserSystemRolePara { SysID = x }).ToList();

            await _userSystemRepository.EditUserOutSourcingSystemRoles(para.UserID, para.UpdUserID, sysIDList);
        }
    }
}