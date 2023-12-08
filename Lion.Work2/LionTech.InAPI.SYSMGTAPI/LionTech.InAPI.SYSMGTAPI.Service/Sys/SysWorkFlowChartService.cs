using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysWorkFlowChartService: ISysWorkFlowChartService
    {
        private readonly ISysWorkFlowChartRepository _sysWorkFlowChartRepository;

        public SysWorkFlowChartService(ISysWorkFlowChartRepository sysWorkFlowChartRepository)
        {
            _sysWorkFlowChartRepository = sysWorkFlowChartRepository;
        }

        public async Task<IEnumerable<SystemWorkFlowNodePosition>> GetSystemWorkFlowNodePositions(string sysID, string wfFlowID, string wfFlowVer)
        {
            return await _sysWorkFlowChartRepository.GetSystemWorkFlowNodePositions(sysID, wfFlowID, wfFlowVer);
        }

        public async Task<IEnumerable<SystemWorkFlowArrowPosition>> GetSystemWorkFlowArrowPositions(string sysID, string wfFlowID, string wfFlowVer)
        {
            return await _sysWorkFlowChartRepository.GetSystemWorkFlowArrowPositions(sysID, wfFlowID, wfFlowVer);
        }
    }
}