using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysWorkFlowRepository
    {
        Task<IEnumerable<SystemWorkFlowGroup>> GetSystemWorkFlowGroups(string sysID, string cultureID);
        Task<SystemWorkFlowGroupDetail> GetSystemWorkFlowGroupDetail(string sysID, string wfFlowGroupID);
        Task<bool> EditSystemWorkFlowGroupDetail(SystemWorkFlowGroupDetail systemWorkFlowGroupDetail);
        Task<bool> CheckSystemWorkFlowGroupExists(string sysID, string wfFlowGroupID);
        Task<bool> CheckSystemWorkFlowExists(string sysID, string wfFlowGroupID);
        Task<bool> DeleteSystemWorkFlowGroupDetail(string sysID, string wfFlowGroupID);
        Task<IEnumerable<SystemWorkFlowNode>> GetSystemWorkFlowNodes(string sysID, string cultureID, string wfFlowID, string wfFlowVer);
        Task<IEnumerable<SysUserSystemWorkFlowID>> GetSysUserSystemWorkFlowIDs(string sysID, string userID, string cultureID, string wfFlowGroupID);
        Task<IEnumerable<SystemWorkFlow>> GetSystemWorkFlows(string sysID, string wfFlowGroupID, string cultureID);
        Task<SystemWorkFlowDetail> GetSystemWorkFlowDetail(string sysID, string wfFlowID, string wfFlowVer);
        Task<bool> CheckWorkFlowIsExists(string sysID, string wfFlowID, string wfFlowVer);
        Task<IEnumerable<FlowRole>> GetSystemWorkFlowRoles(string sysId, string wfFlowID, string wfFlowVer, string cultureID);
        Task<bool> EditSystemWorkFlowDetail(SystemWorkFlowDetails systemWorkFlowDetails);
        Task<bool> DeleteSystemWorkFlowDetail(string sysID, string wfFlowID, string wfFLowVer);
        Task<SystemWorkFlowNode> GetSystemWorkFlowNode(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID);
        Task<IEnumerable<SystemWorkFlowGroupIDs>> GetSystemWorkFlowGroupIDs(string sysID, string cultureID);
    }
}
