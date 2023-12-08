using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysWorkFlowNodeService
    {
        Task<SystemWorkFlow> GetSystemWorkFlowName(string sysID, string wfFlowID, string wfFlowVer, string cultureID);
        Task<IEnumerable<SystemWorkFlowNode>> GetBackSystemWorkFlowNodes(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID);
        Task<IEnumerable<SystemWorkFlowNodeRole>> GetSystemWorkFlowNodeRoles(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID);
        Task<SystemWorkFlowNodeDetailExecuteResult> GetSystemWorkFlowNode(string sysid, string wfflowid, string wfflowver, string wfnodeid);
        Task<IEnumerable<SystemWorkFlowNodeDetailExecuteResult>> GetSystemWorkFlowNodes(string sysID, string wfFlowID, string wfFlowVer);
        Task<SystemWorkFlowNodeDetailExecuteResult> EditSystemWorkFlowNode(SystemWorkFlowNodePara para);
        Task<bool> IsWorkFlowChildsExist(string sysID, string wfFlowID, string wfFlowVer, string wfnodeid);
        Task DeleteSystemWorkFlowNodeDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID);
        Task<bool> IsWorkFlowHasRunTime(string sysID, string wfFlowID, string wfFlowVer);
        Task<IEnumerable<SystemWorkFlowNodeIDs>> GetSystemWorkFlowNodeIDs(string sysID, string wfFlowID, string wfFlowVer, string cultureID);
    }
}
