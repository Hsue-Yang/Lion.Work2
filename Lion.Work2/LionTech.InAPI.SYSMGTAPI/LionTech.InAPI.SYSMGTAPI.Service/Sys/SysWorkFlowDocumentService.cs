using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysWorkFlowDocumentService : ISysWorkFlowDocumentService
    {
        private readonly ISysWorkFlowDocumentRepository _systemWorkFlowDocumentRepository;

        public SysWorkFlowDocumentService(ISysWorkFlowDocumentRepository systemWorkFlowDocumentRepository)
        {
            _systemWorkFlowDocumentRepository = systemWorkFlowDocumentRepository;
        }
        public Task<IEnumerable<SystemWorkFlowDocument>> GetSystemWorkFlowDocuments(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID)
        {
            return _systemWorkFlowDocumentRepository.GetSystemWorkFlowDocuments(sysID, wfFlowID, wfFlowVer, wfNodeID, cultureID);
        }

        public Task<SystemWorkFlowDocumentDetail> GetSystemWorkFlowDocumentDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfDocSeq)
        {
            return _systemWorkFlowDocumentRepository.GetSystemWorkFlowDocumentDetail(sysID, wfFlowID, wfFlowVer, wfNodeID, wfDocSeq);
        }

        public Task InsertSystemWorkFlowDocument(SystemWorkFlowDocumentDetail systemWorkFlowDocumentDetail)
        {
            return _systemWorkFlowDocumentRepository.InsertSystemWorkFlowDocument(systemWorkFlowDocumentDetail);
        }

        public Task EditSystemWorkFlowDocument(SystemWorkFlowDocumentDetail systemWorkFlowDocumentDetail)
        {
            return _systemWorkFlowDocumentRepository.EditSystemWorkFlowDocument(systemWorkFlowDocumentDetail);
        }

        public Task<string> DeleteSystemWorkFlowDocument(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfDocSeq)
        {
            return _systemWorkFlowDocumentRepository.DeleteSystemWorkFlowDocument(sysID, wfFlowID, wfFlowVer, wfNodeID, wfDocSeq);
        }
    }
}
