using LionTech.InAPI.SYSMGTAPI.Domain.Entities;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysEDIRepository
    {
        Task<IEnumerable<SystemEDIFlow>> GetSystemEDIFlows(string sysID, string schFrequency, string cultureID);

        Task EditEDIFlowSettingSort(List<SystemEDIFlowSort> SystemEDIFlowSort);

        Task<string> GetSystemEDIIPAddress(string sysID);

        Task<SystemEDIFlowDetails> GetSystemEDIFlowDetail(string sysID, string ediFlowID);

        Task EditSystemEDIFlowDetail(SystemEDIFlowDetail systemEDIFlowDetail);

        Task<EnumDeleteResult> DeleteSystemEDIFlowDetail(string sysID, string ediFlowID);

        Task<string> GetFlowNewSortOrder(string sysID);

        Task<(int rowCount, IEnumerable<SystemEDIJobLog> systemEDIJobLogList)> GetSystemEDIJobLogs(string sysID, string ediNO, string ediFlowID, string ediJobID, string ediFlowIDSearch, string ediJobIDSearch, string edidate, string cultureID, int pageIndex, int pageSize);
        
        Task<IEnumerable<SystemEDIJob>> GetSystemEDIJobs(string sysID, string ediFlowID, string ediJobType, string cultureID);
        
        Task<SystemEDIJob> GetSystemEDIJobDetail(string sysID, string ediFlowID, string ediJobID);
        
        Task<IEnumerable<SystemDepEDIJobID>> GetSystemEDIJobByIds(string sysID, string ediFlowID, string cultureID);
        
        Task<string> GetJobMaxSortOrder(string sysID, string ediFlowID);
        
        Task<IEnumerable<SystemEDIPara>> GetSystemEDIParas(string sysID, string ediFlowID, string ediJobID, string cultureID);
        
        Task EditSystemEDIJobSortOrder(List<EditEDIJobValue> editSystemEDIFlow);
        
        Task EditSystemEDIJobDetail(EditEDIJobDetail systemEDIJobDetail);
        
        Task EditSystemEDIJobImport(EdiJobSettingPara ediJobSettingPara);
        
        Task<EnumDeleteResult> DeleteSystemEDIJobDetail(string sysID, string ediflowID, string ediJobID);
        
        Task EditSystemEDIParaSortOrder(List<EditEDIJobPara> editEDIJobParas);
        
        Task EditSystemEDIPara(EditEDIJobPara editEDIJobParas);
        
        Task DeleteSystemEDIPara(string sysID, string ediFlowID, string ediJobID, string ediJobParaID);
        
        Task<IEnumerable<SystemEDICon>> GetSystemEDIConByIdsProviderCons(string sysID, string ediflowID);
        
        Task<IEnumerable<SystemEDICon>> GetSystemEDIConByIds(string sysID, string ediflowID, string cultureID);

        Task<(int rowCount, IEnumerable<SystemEDIFlowLog> systemEDIFlowLogList)> GetSystemEDIFlowLogs(string sysID, string ediNO, string ediFlowID, string ediDate, string dataDate, string resultID, string statusID, string cultureID, int pageIndex, int pageSize);
        
        Task EditEDIFlowLogs(SystemEDIFlowLogUpdateWaitStatus systemEDIFlowLogUpdateWaitStatus);
        
        Task<IEnumerable<SystemEDICon>> GetSystemEDICons(string sysID, string cultureID, string ediflowID);

        Task EditEDIConSort(List<SystemEDICon> systemEDICon);

        Task<SystemEDIConDetail> GetSystemEDIConDetail(string sysID, string ediflowID, string ediconID);
        
        Task EditSystemEDIConDetail(SystemEDIConDetail systemEDIConDetail);
        
        Task<EnumDeleteResult> DeleteSystemEDIConDetail(string sysID, string ediflowID, string ediconID);
        
        Task<string> GetConNewSortOrder(string sysID, string ediflowID);
        
        Task InsertSystemEDIFlowLog(SystemEDILogSetting systemEDILogSetting);

        Task<IEnumerable<SystemEDIFlowSchedule>> GetSystemEDIFlowScheduleList(string sysID, string cultureID);
        
        Task<IEnumerable<SystemEDIFlowByIds>> GetSystemEDIFlowByIds(string sysID, string cultureID);
        
        Task<SystemEDIXML> GetSystemEDIFlowXMLDetail(string sysID, string cultureID);
    }
}
