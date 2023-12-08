using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysWorkFlowSignatureService
    {
        Task<IEnumerable<SystemWFSig>> GetSystemWorkFlowSignatures(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID);
        Task<ReturnSystemWFNode> EditSystemWorkFlowNode(SystemWFNode systemWFNode);
        Task<IEnumerable<SystemWFSigSeq>> GetSystemWorkFlowSignatureSeqs(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID);
        Task<SystemWorkFlowSignatureDetail> GetSystemWorkFlowSignatureDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfSigSeq);
        Task<IEnumerable<SystemRoleSig>> GetSystemRoleSignatures(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfSigSeq, string cultureID);
        Task InsertSystemWorkFlowSignatureDetail(SystemWFSignatureDetail systemWFSignatureDetail);
        Task EditSystemWorkFlowSignatureDetail(SystemWFSignatureDetail systemWFSignatureDetail);
        Task DeleteSystemWorkFlowSignatureDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfSigSeq);
    }
}
