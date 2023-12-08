using System.Collections.Generic;
using System.Threading.Tasks;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class LatestUseIPService : ILatestUseIPService
    {
        private readonly ILatestUseIPRepository _latestuseipRepository;

        public LatestUseIPService(ILatestUseIPRepository latestuseipRepository)
        {
            _latestuseipRepository = latestuseipRepository;
        }

        public async Task<(int rowCount, IEnumerable<LatestUseIPInfo> LatestUseIPInfoLists)> GetLatestUseIPInfoList(string ipAddress, string codeNM, int pageIndex, int pageSize)
        {
           return await _latestuseipRepository.SelectLatestUseIPInfoList(ipAddress, codeNM, pageIndex, pageSize);
        }
    }
}
