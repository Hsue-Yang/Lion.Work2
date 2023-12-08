using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysRecordService : ISysRecordService
    {
        private readonly ISysRecordRepository _sysRecordRepository;
        public SysRecordService(ISysRecordRepository sysRecordRepository)
        {
            _sysRecordRepository = sysRecordRepository;
        }

        public async Task<IEnumerable<object>> GetSystemRecord(LogPara logPara)
        {
            if (logPara.CollectionNM == "LOG_USER_SYSTEM_ROLE")
            {
                return await _sysRecordRepository.GetLogUserSystemRoles(logPara);
            }
            return await _sysRecordRepository.GetSysRecordMongoDB(logPara);
        }
    }
}
