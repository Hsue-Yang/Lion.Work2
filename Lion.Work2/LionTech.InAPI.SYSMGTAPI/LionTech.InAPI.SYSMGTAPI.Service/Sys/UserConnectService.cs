using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class UserConnectService : IUserConnectService
    {
        private readonly IUserConnectRepository _userConnectRepository;

        public UserConnectService(IUserConnectRepository userConnectRepository)
        {
            _userConnectRepository = userConnectRepository;
        }

        public async Task<(int rowCount, IEnumerable<UserConnect> userConnectList)> GetUserConnectList(string connectDTBegin, string connectDTEnd, int pageIndex, int pageSize)
        {
            return await _userConnectRepository.GetUserConnectList(connectDTBegin, connectDTEnd, pageIndex, pageSize);
        }
    }
}
