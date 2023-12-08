using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysWorkFlowNextService : ISysWorkFlowNextService
    {
        private readonly ISysWorkFlowNextRepository _sysWorkFlowNextRepository;

        public SysWorkFlowNextService(ISysWorkFlowNextRepository sysWorkFlowNextRepository)
        {
            _sysWorkFlowNextRepository = sysWorkFlowNextRepository;
        }

        public Task<IEnumerable<SystemWorkFlowNext>> GetSystemWorkFlowNext(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID)
        {
            return _sysWorkFlowNextRepository.GetSystemWorkFlowNext(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID);
        }

        public Task<IEnumerable<SystemWFNodeList>> GetSystemWorkFlowNextNodes(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string nodeTypeListstr, string cultureID)
        {
            return _sysWorkFlowNextRepository.GetSystemWorkFlowNextNodes(sysID, wfFlowID, wfFlowVer, wfNodeID, nodeTypeListstr, cultureID);
        }

        public Task<SystemWFNext> GetSystemWorkFlowNextDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string nextWFNodeID, string cultureID)
        {
            return _sysWorkFlowNextRepository.GetSystemWorkFlowNextDetail(sysID, wfFlowID, wfFlowVer, wfNodeID, nextWFNodeID, cultureID);
        }

        public Task EditSystemWorkFlowNext(EditSystemWFNext editSystemWFNext)
        {
            return _sysWorkFlowNextRepository.EditSystemWorkFlowNext(editSystemWFNext);
        }
        
        public Task DeleteSystemWorkFlowNext(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string nextWFNodeID)
        {
            return _sysWorkFlowNextRepository.DeleteSystemWorkFlowNext(sysID, wfFlowID, wfFlowVer, wfNodeID, nextWFNodeID);
        }
    }
}
