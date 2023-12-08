using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using System.Linq;


namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysEDIService
    {
        public Task<IEnumerable<SystemEDIFlow>> GetSystemEDIFlows(string sysID, string schFrequency, string cultureID)
        {
            return _sysEDIRepository.GetSystemEDIFlows(sysID, schFrequency, cultureID);
        }

        public Task EditEDIFlowSettingSort(List<SystemEDIFlowSort> systemEDIFlowSort)
        {
            return _sysEDIRepository.EditEDIFlowSettingSort(systemEDIFlowSort);
        }

        public Task<string> GetSystemEDIIPAddress(string sysID)
        {
            return _sysEDIRepository.GetSystemEDIIPAddress(sysID);
        }

        public Task<SystemEDIFlowDetails> GetSystemEDIFlowDetail(string sysID, string ediFlowID)
        {
            return _sysEDIRepository.GetSystemEDIFlowDetail(sysID, ediFlowID);
        }

        public Task EditSystemEDIFlowDetail(SystemEDIFlowDetail systemEDIFlowDetail)
        {
            return _sysEDIRepository.EditSystemEDIFlowDetail(systemEDIFlowDetail);
        }

        public Task<EnumDeleteResult> DeleteSystemEDIFlowDetail(string sysID, string ediFlowID)
        {
            return _sysEDIRepository.DeleteSystemEDIFlowDetail(sysID, ediFlowID);
        }

        public Task<string> GetFlowNewSortOrder(string sysID)
        {
            return _sysEDIRepository.GetFlowNewSortOrder(sysID);
        }
        public Task<IEnumerable<SystemEDIFlowSchedule>> GetSystemEDIFlowScheduleList(string sysID, string cultureID)
        {
            return _sysEDIRepository.GetSystemEDIFlowScheduleList(sysID, cultureID);
        }
        public Task<IEnumerable<SystemEDIFlowByIds>> GetSystemEDIFlowByIds(string sysID, string cultureID)
        {
            return _sysEDIRepository.GetSystemEDIFlowByIds(sysID, cultureID);
        }

        public async Task<SystemEDIXML> GetSystemEDIFlowXMLDetail(string sysID, string cultureID)
        {
            var result = await _sysEDIRepository.GetSystemEDIFlowXMLDetail(sysID, cultureID);
            result.SystemEDIFlowDetails.Select(flow =>
            {
                flow.SCHStartTime = FormatTime(flow.SCHStartTime);
                return flow;
            }).ToList();

            result.SystemEDIFlowExecuteTimeDetails.Select(fixedTime =>
            {
                fixedTime.ExecuteTime = FormatTime(fixedTime.ExecuteTime);
                return fixedTime;
            }).ToList();

            return result;
        }
        
        private string FormatTime(string time) => $"{time.Substring(0, 2)}:{time.Substring(2, 2)}:{time.Substring(4, 2)}.{time.Substring(6)}";
    }
}
