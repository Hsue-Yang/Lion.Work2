using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities;



namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysEDIService
    {
        public Task<(int rowCount, IEnumerable<SystemEDIFlowLog> systemEDIFlowLogList)> GetSystemEDIFlowLogs(string sysID, string ediNO, string ediFlowID, string ediDate, string dataDate, string resultID, string statusID, string cultureID, int pageIndex, int pageSize)
        {
            return _sysEDIRepository.GetSystemEDIFlowLogs(sysID, ediNO, ediFlowID, ediDate, dataDate, resultID, statusID, cultureID, pageIndex, pageSize);
        }

        public Task EditEDIFlowLogs(SystemEDIFlowLogUpdateWaitStatus systemEDIFlowLogs)
        {
            return _sysEDIRepository.EditEDIFlowLogs(systemEDIFlowLogs);
        }
    }
}

