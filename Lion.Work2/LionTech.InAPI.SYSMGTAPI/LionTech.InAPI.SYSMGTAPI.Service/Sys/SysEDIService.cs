using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;


namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysEDIService : ISysEDIService
    {
        private readonly ISysEDIRepository _sysEDIRepository;

        public SysEDIService(ISysEDIRepository sysEDIRepository)
        {
            _sysEDIRepository = sysEDIRepository;
        }
    }
}
