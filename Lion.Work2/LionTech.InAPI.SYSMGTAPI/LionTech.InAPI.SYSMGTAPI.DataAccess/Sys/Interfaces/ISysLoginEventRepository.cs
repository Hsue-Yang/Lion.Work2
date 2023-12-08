using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysLoginEventRepository
    {
        Task<IEnumerable<SysLoginEventID>> GetSysLoginEventListById(string sysID, string cultureID);
        Task<(int rowCount, IEnumerable<SysLoginEventSetting>)> GetSysLoginEventSettingList(string sysID, string cultureID, string logineventID, int pageIndex, int pageSize);
        Task<LoginEventSettingDetail> GetLoginEventSettingDetail(string sysID, string logineventID);
        Task EditSysLoginEventSettingSort(List<LoginEventSettingValue> loginEventSettingValue);
        Task EditLoginEventSettingDetail(LoginEventSettingDetail loginEventSettingDetail);
        Task DeleteLoginEventSettingDetail(string sysID, string logineventID);

        Task<bool> CheckSysLoginEventIdIsExists(string sysID, string logineventID);
    }
}
