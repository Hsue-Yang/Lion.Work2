using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysWorkFlowNodeRepository
    {
        Task<SystemWorkFlow> GetSystemWorkFlowName(string sysID, string wfFlowID, string wfFlowVer, string cultureID);
        Task<IEnumerable<SystemWorkFlowNode>> GetBackSystemWorkFlowNodes(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID);
        Task<IEnumerable<SystemWorkFlowNodeRole>> GetSystemWorkFlowNodeRoles(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID);
        Task<SystemWorkFlowNodeDetailExecuteResult> GetSystemWorkFlowNodeDetail(string sysid, string wfflowid, string wfflowver, string wfnodeid);
        Task<IEnumerable<SystemWorkFlowNodeDetailExecuteResult>> GetSystemWorkFlowNodeDetails(string sysID, string wfFlowID, string wfFlowVer);
        Task<SystemWorkFlowNodeDetailExecuteResult> EditSystemWorkFlowNodeDetail(SystemWorkFlowNodePara para);
        Task<bool> IsWorkFlowChildsExist(string sysID, string wfFlowID, string wfFlowVer, string wfnodeid);
        Task DeleteSystemWorkFlowNodeDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID);
        Task<bool> IsWorkFlowHasRunTime(string sysID, string wfFlowID, string wfFlowVer);
        Task<IEnumerable<SystemWorkFlowNodeIDs>> GetSystemWorkFlowNodeIDs(string sysID, string wfFlowID, string wfFlowVer, string cultureID);
    }
}
