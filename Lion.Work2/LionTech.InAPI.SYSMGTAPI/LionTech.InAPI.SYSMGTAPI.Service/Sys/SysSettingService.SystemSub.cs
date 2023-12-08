using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysSettingService
    {
        public Task<IEnumerable<SystemSub>> GetUserSystemSubList(string userID, string cultureID)
        {
            return _sysSettingRepository.GetUserSystemSubList(userID, cultureID);
        }

        public Task<IEnumerable<SystemSetting.SystemSub>> GetSystemSubByIdList(string sysID, string cultureID)
        {
            return _sysSettingRepository.GetSystemSubByIdList(sysID, cultureID);
        }

        public Task<IEnumerable<SystemSub>> GetSystemSubList(string sysID)
        {
            return _sysSettingRepository.GetSystemSubList(sysID);
        }

        public Task EditSystemSubDetail(SystemSub systemSub, string userID)
        {
            systemSub.UpdUserID = userID;
            return _sysSettingRepository.EditSystemSubDetail(systemSub);
        }

        public Task DeleteSystemSubDetail(string sysID)
        {
            return _sysSettingRepository.DeleteSystemSubDetail(sysID);
        }
    }
}