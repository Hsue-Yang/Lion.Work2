using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysRecordRepository
    {
        Task<IEnumerable<LogUserSystemRole>> GetLogUserSystemRoles(LogPara logPara);
        Task<IEnumerable<SysRecord>> GetSysRecordMongoDB(LogPara logPara);
    }
}
