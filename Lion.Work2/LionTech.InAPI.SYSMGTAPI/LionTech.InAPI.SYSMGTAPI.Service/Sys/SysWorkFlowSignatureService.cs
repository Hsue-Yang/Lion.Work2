using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysWorkFlowSignatureService : ISysWorkFlowSignatureService
    {
        private readonly ISysWorkFlowSignatureRepository _sysWorkFlowSignatureRepository;

        public SysWorkFlowSignatureService(ISysWorkFlowSignatureRepository sysWorkFlowSignatureRepository)
        {
            _sysWorkFlowSignatureRepository = sysWorkFlowSignatureRepository;
        }

        public async Task<IEnumerable<SystemWFSig>> GetSystemWorkFlowSignatures(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID)
        {
            return await _sysWorkFlowSignatureRepository.GetSystemWorkFlowSignatures(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID);
        }

        public async Task<ReturnSystemWFNode> EditSystemWorkFlowNode(SystemWFNode systemWFNode)
        {
            return await _sysWorkFlowSignatureRepository.EditSystemWorkFlowNode(systemWFNode);
        }

        public async Task<IEnumerable<SystemWFSigSeq>> GetSystemWorkFlowSignatureSeqs(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID)
        {
            return await _sysWorkFlowSignatureRepository.GetSystemWorkFlowSignatureSeqs(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID);
        }

        public async Task<SystemWorkFlowSignatureDetail> GetSystemWorkFlowSignatureDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfSigSeq)
        {
            return await _sysWorkFlowSignatureRepository.GetSystemWorkFlowSignatureDetail(sysID, wfFlowID, wfFlowVer, wfNodeID, wfSigSeq);
        }

        public async Task<IEnumerable<SystemRoleSig>> GetSystemRoleSignatures(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfSigSeq, string cultureID)
        {
            return await _sysWorkFlowSignatureRepository.GetSystemRoleSignatures(sysID, wfFlowID, wfFlowVer, wfNodeID, wfSigSeq, cultureID);
        }

        public async Task InsertSystemWorkFlowSignatureDetail(SystemWFSignatureDetail systemWFSignatureDetail)
        {
            await _sysWorkFlowSignatureRepository.InsertSystemWorkFlowSignatureDetail(systemWFSignatureDetail);
        }

        public async Task EditSystemWorkFlowSignatureDetail(SystemWFSignatureDetail systemWFSignatureDetail)
        {
            await _sysWorkFlowSignatureRepository.EditSystemWorkFlowSignatureDetail(systemWFSignatureDetail);
        }

        public async Task DeleteSystemWorkFlowSignatureDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfSigSeq)
        {
            await _sysWorkFlowSignatureRepository.DeleteSystemWorkFlowSignatureDetail(sysID, wfFlowID, wfFlowVer, wfNodeID, wfSigSeq);
        }
    }
}
