using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysEDIService
    {
        public async Task<IEnumerable<SystemEDIJob>> GetSystemEDIJobs(string sysID, string ediFlowID, string ediJobType, string cultureID)
        {
            return await _sysEDIRepository.GetSystemEDIJobs(sysID, ediFlowID, ediJobType, cultureID);
        }

        public async Task<SystemEDIJob> GetSystemEDIJobDetail(string sysID, string ediFlowID, string ediJobID)
        {
            return await _sysEDIRepository.GetSystemEDIJobDetail(sysID, ediFlowID, ediJobID);
        }

        public async Task<IEnumerable<SystemDepEDIJobID>> GetSystemEDIJobByIds(string sysID, string ediFlowID, string cultureID)
        {
            return await _sysEDIRepository.GetSystemEDIJobByIds(sysID, ediFlowID, cultureID);
        }

        public async Task<string> GetJobMaxSortOrder(string sysID, string ediFlowID)
        {
            return await _sysEDIRepository.GetJobMaxSortOrder(sysID, ediFlowID);
        }

        public async Task<IEnumerable<SystemEDIPara>> GetSystemEDIParas(string sysID, string ediFlowID, string ediJobID, string cultureID)
        {
            return await _sysEDIRepository.GetSystemEDIParas(sysID, ediFlowID, ediJobID, cultureID);
        }
       
        public async Task EditSystemEDIJobSortOrder(List<EditEDIJobValue> editSystemEDIFlow)
        {
            await _sysEDIRepository.EditSystemEDIJobSortOrder(editSystemEDIFlow);
        }
        
        public async Task EditSystemEDIJobDetail(EditEDIJobDetail editEDIJobDetail)
        {
            await _sysEDIRepository.EditSystemEDIJobDetail(editEDIJobDetail);
        }
   
        public async Task EditSystemEDIJobImport(EdiJobSettingPara ediJobSettingPara)
        {
            await _sysEDIRepository.EditSystemEDIJobImport(ediJobSettingPara);
        }
      
        public async Task<EnumDeleteResult> DeleteSystemEDIJobDetail(string sysID, string ediflowID, string ediJobID)
        {
            return await _sysEDIRepository.DeleteSystemEDIJobDetail(sysID, ediflowID, ediJobID);
        }
 
        public async Task EditSystemEDIParaSortOrder(List<EditEDIJobPara> editEDIJobParas)
        {
            await _sysEDIRepository.EditSystemEDIParaSortOrder(editEDIJobParas);
        }

        public async Task EditSystemEDIPara(EditEDIJobPara editEDIJobParas)
        {
            await _sysEDIRepository.EditSystemEDIPara(editEDIJobParas);
        }
  
        public async Task DeleteSystemEDIPara(string sysID, string ediFlowID, string ediJobID, string ediJobParaID)
        {
            await _sysEDIRepository.DeleteSystemEDIPara(sysID, ediFlowID, ediJobID, ediJobParaID);
        }
   
        public Task<IEnumerable<SystemEDICon>> GetSystemEDIConByIdsProviderCons(string sysID, string ediflowID)
        {
            return _sysEDIRepository.GetSystemEDIConByIdsProviderCons(sysID, ediflowID);
        }
    
        public Task<IEnumerable<SystemEDICon>> GetSystemEDIConByIds(string sysID, string ediflowID, string cultureID)
        {
            return _sysEDIRepository.GetSystemEDIConByIds(sysID, ediflowID, cultureID);
        }
    }
}
