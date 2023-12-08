using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysWorkFlowNodeService : ISysWorkFlowNodeService
    {
        private readonly ISysWorkFlowNodeRepository _sysWorkFlowNodeRepository;

        public SysWorkFlowNodeService(ISysWorkFlowNodeRepository sysWorkFlowNodeRepository)
        {
            _sysWorkFlowNodeRepository = sysWorkFlowNodeRepository;
        }

        public async Task<SystemWorkFlow> GetSystemWorkFlowName(string sysID, string wfFlowID, string wfFlowVer, string cultureID)
        {
            return await _sysWorkFlowNodeRepository.GetSystemWorkFlowName(sysID, wfFlowID, wfFlowVer, cultureID);
        }
        public async Task<IEnumerable<SystemWorkFlowNode>> GetBackSystemWorkFlowNodes(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID)
        {
            return await _sysWorkFlowNodeRepository.GetBackSystemWorkFlowNodes(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID);
        }
        public async Task<IEnumerable<SystemWorkFlowNodeRole>> GetSystemWorkFlowNodeRoles(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID)
        {
            return await _sysWorkFlowNodeRepository.GetSystemWorkFlowNodeRoles(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID);
        }
        public async Task<SystemWorkFlowNodeDetailExecuteResult> GetSystemWorkFlowNode(string sysid, string wfflowid, string wfflowver, string wfnodeid)
        {
            return await _sysWorkFlowNodeRepository.GetSystemWorkFlowNodeDetail(sysid, wfflowid, wfflowver, wfnodeid);
        }
        public async Task<IEnumerable<SystemWorkFlowNodeDetailExecuteResult>> GetSystemWorkFlowNodes(string sysID, string wfFlowID, string wfFlowVer)
        {
            return await _sysWorkFlowNodeRepository.GetSystemWorkFlowNodeDetails(sysID, wfFlowID, wfFlowVer);
        }
        public async Task<SystemWorkFlowNodeDetailExecuteResult> EditSystemWorkFlowNode(SystemWorkFlowNodePara para)
        {
            return await _sysWorkFlowNodeRepository.EditSystemWorkFlowNodeDetail(para);
        }
        public async Task<bool> IsWorkFlowChildsExist(string sysID, string wfFlowID, string wfFlowVer, string wfnodeid)
        {
            return await _sysWorkFlowNodeRepository.IsWorkFlowChildsExist(sysID, wfFlowID, wfFlowVer, wfnodeid);
        }
        public async Task DeleteSystemWorkFlowNodeDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID)
        {
            await _sysWorkFlowNodeRepository.DeleteSystemWorkFlowNodeDetail(sysID, wfFlowID, wfFlowVer, wfNodeID);
        }
        public async Task<bool> IsWorkFlowHasRunTime(string sysID, string wfFlowID, string wfFlowVer)
        {
            return await _sysWorkFlowNodeRepository.IsWorkFlowHasRunTime(sysID, wfFlowID, wfFlowVer);
        }
        public async Task<IEnumerable<SystemWorkFlowNodeIDs>> GetSystemWorkFlowNodeIDs(string sysID, string wfFlowID, string wfFlowVer, string cultureID)
        {
            return await _sysWorkFlowNodeRepository.GetSystemWorkFlowNodeIDs(sysID, wfFlowID, wfFlowVer, cultureID);
        }
    }
}