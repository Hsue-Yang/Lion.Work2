using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysWorkFlowDocumentService
    {
        Task<IEnumerable<SystemWorkFlowDocument>> GetSystemWorkFlowDocuments(string sysID, string wfFlowID,string wfFlowVer, string wfNodeID, string cultureID);
        Task<SystemWorkFlowDocumentDetail> GetSystemWorkFlowDocumentDetail(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfDocSeq);
        Task InsertSystemWorkFlowDocument(SystemWorkFlowDocumentDetail systemWorkFlowDocumentDetail);
        Task EditSystemWorkFlowDocument(SystemWorkFlowDocumentDetail systemWorkFlowDocumentDetail);
        Task<string> DeleteSystemWorkFlowDocument(string sysID, string wfFlowID, string wfFlowVer, string wfNodeID, string wfDocSeq);
    }
}
