using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysWorkFlowService
    {
        public Task<IEnumerable<SystemWorkFlowNode>> GetSystemWorkFlowNodes(string sysID, string cultureID, string wfFlowID, string wfFlowVer)
        {
            return _sysWorkFlowRepository.GetSystemWorkFlowNodes(sysID, cultureID, wfFlowID, wfFlowVer);
        }

        public async Task<SystemWorkFlowNode> GetSystemWorkFlowNode(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID)
        {
            return await _sysWorkFlowRepository.GetSystemWorkFlowNode(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID);
        }
    }
}