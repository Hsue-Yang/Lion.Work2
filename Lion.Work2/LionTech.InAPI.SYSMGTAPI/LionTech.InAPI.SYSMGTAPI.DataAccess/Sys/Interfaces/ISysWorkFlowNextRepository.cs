using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysWorkFlowNextRepository
    {
        Task<IEnumerable<SystemWorkFlowNext>> GetSystemWorkFlowNext(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID);
        Task<IEnumerable<SystemWFNodeList>> GetSystemWorkFlowNextNodes(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string nodeType, string cultureID);
        Task<SystemWFNext> GetSystemWorkFlowNextDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string nextWFNodeID, string cultureID);
        Task EditSystemWorkFlowNext(EditSystemWFNext editSystemWFNext);
        Task DeleteSystemWorkFlowNext(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string nextWFNodeID);
    }
}
