using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysEDIService
    {
        public Task<(int rowCount, IEnumerable<SystemEDIJobLog> systemEDIJobLogList)> GetSystemEDIJobLogs(string sysID, string ediNO, string ediFlowID, string ediJobID, string ediFlowIDSearch, string ediJobIDSearch, string edidate, string cultureID, int pageIndex, int pageSize)
        {
            return _sysEDIRepository.GetSystemEDIJobLogs(sysID, ediNO, ediFlowID, ediJobID, ediFlowIDSearch, ediJobIDSearch, edidate, cultureID, pageIndex, pageSize);
        }
    }
}
