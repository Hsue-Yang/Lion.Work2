using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysWorkFlowNextService
    {
        Task<IEnumerable<SystemWorkFlowNext>> GetSystemWorkFlowNext(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID);
        Task<IEnumerable<SystemWFNodeList>> GetSystemWorkFlowNextNodes(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string nodeTypeListstr, string cultureID);
        Task<SystemWFNext> GetSystemWorkFlowNextDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string nextWFNodeID, string cultureID);
        Task EditSystemWorkFlowNext(EditSystemWFNext editSystemWFNext);
        Task DeleteSystemWorkFlowNext(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string nextWFNodeID);
    }
}
