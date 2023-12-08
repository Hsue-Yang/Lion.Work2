using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysWorkFlowService : ISysWorkFlowService
    {
        private readonly ISysWorkFlowRepository _sysWorkFlowRepository;

        public SysWorkFlowService(ISysWorkFlowRepository sysWorkFlowRepository)
        {
            _sysWorkFlowRepository = sysWorkFlowRepository;
        }

        public Task<IEnumerable<SystemWorkFlow>> GetSystemWorkFlows(string sysID, string wfFlowGroupID, string cultureID)
        {
            return _sysWorkFlowRepository.GetSystemWorkFlows(sysID, wfFlowGroupID, cultureID);
        }

        public Task<SystemWorkFlowDetail> GetSystemWorkFlowDetail(string sysID, string wfFlowID, string wfFlowVer)
        {
            return _sysWorkFlowRepository.GetSystemWorkFlowDetail(sysID, wfFlowID, wfFlowVer);
        }

        public Task<bool> CheckWorkFlowIsExists(string sysID, string wfFlowID, string wfFlowVer)
        {
            return _sysWorkFlowRepository.CheckWorkFlowIsExists(sysID, wfFlowID, wfFlowVer);
        }

        public Task<IEnumerable<FlowRole>> GetSystemWorkFlowRoles(string sysId, string wfFlowID, string wfFlowVer, string cultureID)
        {
            return _sysWorkFlowRepository.GetSystemWorkFlowRoles(sysId, wfFlowID, wfFlowVer, cultureID);
        }

        public Task<bool> EditSystemWorkFlowDetail(SystemWorkFlowDetails systemWorkFlowDetails)
        {
            return _sysWorkFlowRepository.EditSystemWorkFlowDetail(systemWorkFlowDetails);
        }

        public Task<bool> DeleteSystemWorkFlowDetail(string sysID, string wfFlowID, string wfFlowVer)
        {
            return _sysWorkFlowRepository.DeleteSystemWorkFlowDetail(sysID, wfFlowID, wfFlowVer);
        }

        public Task<IEnumerable<SysUserSystemWorkFlowID>> GetSysUserSystemWorkFlowIDs(string sysID, string userID, string cultureID, string wfFlowGroupID)
        {
            return _sysWorkFlowRepository.GetSysUserSystemWorkFlowIDs(sysID, userID, cultureID, wfFlowGroupID);
        }
    }
}
