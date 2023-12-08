using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysWorkFlowDocumentRepository
    {
        Task<IEnumerable<SystemWorkFlowDocument>> GetSystemWorkFlowDocuments(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string cultureID);
        Task<SystemWorkFlowDocumentDetail> GetSystemWorkFlowDocumentDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfDocSeq);
        Task InsertSystemWorkFlowDocument(SystemWorkFlowDocumentDetail systemWorkFlowDocumentDetail);
        Task EditSystemWorkFlowDocument(SystemWorkFlowDocumentDetail systemWorkFlowDocumentDetail);
        Task<string> DeleteSystemWorkFlowDocument(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfDocSeq);
    }
}
