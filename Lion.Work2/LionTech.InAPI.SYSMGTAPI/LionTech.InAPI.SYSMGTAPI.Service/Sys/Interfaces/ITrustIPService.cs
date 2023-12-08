using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ITrustIPService
    {
        Task<(int rowCount, IEnumerable<TrustIP> TrustIPList)> GetTrustIPs(TrustIPPara para);
        Task<TrustIP> GetTrustIPDetail(string IPBegin, string IPEnd, string cultureID);
        Task<bool> GetValidTrustIPRepeated(TrustIP trustIP);
        Task<string> EditTrustIP(TrustIP trustIP);
        Task<string> DeleteTrustIP(string IPBegin, string IPEnd);
        Task InsertTrustIPMongoDB(RecordSysTrustIP para);
    }
}