using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysUserPurviewService : ISysUserPurviewService
    {
        private readonly ISysUserPurviewRepository _sysUserPurviewRepository;

        public SysUserPurviewService(ISysUserPurviewRepository sysUserPurviewRepository)
        {
            _sysUserPurviewRepository = sysUserPurviewRepository;
        }

        public async Task<IEnumerable<SysUserPurview>> GetSysUserPurviews(string userID, string updUserID, string cultureID)
        {
            return await _sysUserPurviewRepository.GetSysUserPurviews(userID, updUserID, cultureID);
        }

        public async Task<IEnumerable<PurviewName>> GetPurviewNames(string sysID, string cultureID)
        {
            return await _sysUserPurviewRepository.GetPurviewNames(sysID, cultureID);
        }

        public async Task<IEnumerable<SysUserPurviewDetails>> GetSysUserPurviewDetails(string sysID, string userID, string cultureID)
        {
            return await _sysUserPurviewRepository.GetSysUserPurviewDetails(sysID, userID, cultureID);
        }

        public async Task EditSysUserPurviewDetail(UserPurviewPara userPurviewPara)
        {
            await _sysUserPurviewRepository.EditSysUserPurviewDetail(userPurviewPara);
        }
    }
}
