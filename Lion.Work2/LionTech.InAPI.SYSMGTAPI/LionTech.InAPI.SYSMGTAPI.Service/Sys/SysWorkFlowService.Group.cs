using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysWorkFlowService
    {
        public Task<IEnumerable<SystemWorkFlowGroup>> GetSystemWorkFlowGroups(string sysID, string cultureID)
        {
            return _sysWorkFlowRepository.GetSystemWorkFlowGroups(sysID, cultureID);
        }
        public Task<SystemWorkFlowGroupDetail> GetSystemWorkFlowGroupDetail(string sysID, string wfFlowGroupID)
        {
            return _sysWorkFlowRepository.GetSystemWorkFlowGroupDetail(sysID, wfFlowGroupID);
        }

        public Task<bool> EditSystemWorkFlowGroupDetail(SystemWorkFlowGroupDetail systemWorkFlowGroupDetail)
        {
            return _sysWorkFlowRepository.EditSystemWorkFlowGroupDetail(systemWorkFlowGroupDetail);
        }

        public Task<bool> CheckSystemWorkFlowGroupExists(string sysID, string wfFlowGroupID)
        {
            return _sysWorkFlowRepository.CheckSystemWorkFlowGroupExists(sysID, wfFlowGroupID);
        }

        public Task<bool> CheckSystemWorkFlowExists(string sysID, string wfFlowGroupID)
        {
            return _sysWorkFlowRepository.CheckSystemWorkFlowExists(sysID, wfFlowGroupID);
        }

        public Task<bool> DeleteSystemWorkFlowGroupDetail(string sysID, string wfFlowGroupID)
        {
            return _sysWorkFlowRepository.DeleteSystemWorkFlowGroupDetail(sysID, wfFlowGroupID);
        }
        public Task<IEnumerable<SystemWorkFlowGroupIDs>> GetSystemWorkFlowGroupIDs(string sysID, string cultureID)
        {
            return _sysWorkFlowRepository.GetSystemWorkFlowGroupIDs(sysID, cultureID);
        }
    }
}
