using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysEDIService
    {

        public Task<IEnumerable<SystemEDICon>> GetSystemEDICons(string sysID, string cultureID, string ediflowID)
        {
            return _sysEDIRepository.GetSystemEDICons(sysID, cultureID, ediflowID);
        }

        public Task EditEDIConSort(List<SystemEDICon> systemEDICon)
        {
            return _sysEDIRepository.EditEDIConSort(systemEDICon);
        }

        public Task<SystemEDIConDetail> GetSystemEDIConDetail(string sysID, string ediFlowID, string ediconID)
        {
            return _sysEDIRepository.GetSystemEDIConDetail(sysID, ediFlowID, ediconID);
        }

        public Task EditSystemEDIConDetail(SystemEDIConDetail systemEDIConDetail)
        {
            return _sysEDIRepository.EditSystemEDIConDetail(systemEDIConDetail);
        }
        public Task<EnumDeleteResult> DeleteSystemEDIConDetail(string sysID, string ediFlowID, string ediconID)
        {
            return _sysEDIRepository.DeleteSystemEDIConDetail(sysID, ediFlowID, ediconID);
        }

        public Task<string> GetConNewSortOrder(string sysID, string ediflowID)
        {
            return _sysEDIRepository.GetConNewSortOrder(sysID, ediflowID);
        }
    }
}
