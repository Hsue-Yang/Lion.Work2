using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysWorkFlowChartRepository
    {
        Task<IEnumerable<SystemWorkFlowNodePosition>> GetSystemWorkFlowNodePositions(string sysID, string wfFlowID, string wfFlowVer);
        Task<IEnumerable<SystemWorkFlowArrowPosition>> GetSystemWorkFlowArrowPositions(string sysID, string wfFlowID, string wfFlowVer);
    }
}