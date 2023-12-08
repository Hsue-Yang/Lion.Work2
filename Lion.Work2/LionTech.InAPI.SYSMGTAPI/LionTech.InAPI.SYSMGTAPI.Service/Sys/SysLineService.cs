using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysLineService : ISysLineService
    {
        private readonly ISysLineRepository _sysLineRepository;
        
        public SysLineService(ISysLineRepository sysLineRepository)
        {
            _sysLineRepository = sysLineRepository;
        }

        public Task<bool> CheckSystemLineBotIdIsExists(string sysID, string lineID)
        {
            return _sysLineRepository.CheckSystemLineBotIdIsExists(sysID, lineID);
        }

        public Task<IEnumerable<SystemLine>> GetSystemLineBotIdList(string sysID, string cultureID)
        {
            return _sysLineRepository.GetSystemLineBotIdList(sysID, cultureID);
        }

        public Task<(int rowCount, IEnumerable<SystemLine> systemLineAccountList)> GetSystemLineBotAccountList(string sysID, string lineID, string cultureID, int pageIndex, int pageSize)
        {
            return _sysLineRepository.GetSystemLineBotAccountList(sysID, lineID, cultureID, pageIndex, pageSize);
        }

        public Task<SystemLine> GetSystemLineBotAccountDetail(string sysID, string lineID)
        {
            return _sysLineRepository.GetSystemLineBotAccountDetail(sysID, lineID);
        }

        public Task EditSystemLineBotAccountDetail(SystemLine systemLine)
        {
            return _sysLineRepository.EditSystemLineBotAccountDetail(systemLine);
        }

        public Task DeleteSystemLineById(string sysID, string lineID)
        {
            return _sysLineRepository.DeleteSystemLineById(sysID, lineID);
        }
    }
}
