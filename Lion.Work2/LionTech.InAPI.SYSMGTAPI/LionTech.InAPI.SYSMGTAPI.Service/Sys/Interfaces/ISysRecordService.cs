using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysRecordService
    {
        Task<IEnumerable<object>> GetSystemRecord(LogPara logPara);
    }
}
