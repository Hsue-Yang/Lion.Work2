using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysEDIService
    {
        public Task InsertSystemEDIFlowLog(SystemEDILogSetting systemEDILogSetting)
        {
            return _sysEDIRepository.InsertSystemEDIFlowLog(systemEDILogSetting);
        }
    }
}