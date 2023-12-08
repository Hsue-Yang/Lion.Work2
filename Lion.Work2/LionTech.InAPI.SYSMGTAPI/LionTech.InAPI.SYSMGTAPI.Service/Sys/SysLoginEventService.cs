using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysLoginEventService : ISysLoginEventService
    {
        private readonly ISysLoginEventRepository _sysLoginEventRepository;

        public SysLoginEventService(ISysLoginEventRepository sysLoginEventRepository)
        {
            _sysLoginEventRepository = sysLoginEventRepository;
        }

        public Task<IEnumerable<SysLoginEventID>> GetSysLoginEventListById(string sysID, string cultureID)
        {
            return _sysLoginEventRepository.GetSysLoginEventListById(sysID, cultureID);
        }

        public Task<(int rowCount, IEnumerable<SysLoginEventSetting> sysLoginEventSettingList)> GetSysLoginEventSettingList(string sysID, string cultureID, string logineventID, int pageIndex, int pageSize)
        {
            return _sysLoginEventRepository.GetSysLoginEventSettingList(sysID, cultureID, logineventID, pageIndex, pageSize);
        }

        public Task<LoginEventSettingDetail> GetLoginEventSettingDetail(string sysID, string logineventID)
        {
            return _sysLoginEventRepository.GetLoginEventSettingDetail(sysID, logineventID);
        }
        public Task EditSysLoginEventSettingSort(List<LoginEventSettingValue> loginEventSettingValue)
        {
            return _sysLoginEventRepository.EditSysLoginEventSettingSort(loginEventSettingValue);
        }
        public Task EditLoginEventSettingDetail(LoginEventSettingDetail loginEventSettingDetail)
        {
            return _sysLoginEventRepository.EditLoginEventSettingDetail(loginEventSettingDetail);
        }

        public Task DeleteLoginEventSettingDetail(string sysID, string logineventID)
        {
            return _sysLoginEventRepository.DeleteLoginEventSettingDetail(sysID, logineventID);
        }

        public Task<bool> CheckSysLoginEventIdIsExists(string sysID, string logineventID)
        {
            return _sysLoginEventRepository.CheckSysLoginEventIdIsExists(sysID, logineventID);
        }
    }
}
