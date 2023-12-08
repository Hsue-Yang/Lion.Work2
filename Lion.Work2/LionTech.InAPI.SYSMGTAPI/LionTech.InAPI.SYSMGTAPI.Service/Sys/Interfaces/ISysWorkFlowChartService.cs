using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysWorkFlowChartService
    {
        Task<IEnumerable<SystemWorkFlowNodePosition>> GetSystemWorkFlowNodePositions(string sysID, string wfFlowID, string wfFlowVer);

        Task<IEnumerable<SystemWorkFlowArrowPosition>> GetSystemWorkFlowArrowPositions(string sysID, string wfFlowID, string wfFlowVer);
    }
}