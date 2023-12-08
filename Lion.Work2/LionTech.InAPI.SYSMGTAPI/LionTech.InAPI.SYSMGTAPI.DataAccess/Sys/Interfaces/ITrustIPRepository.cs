using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ITrustIPRepository
    {
        Task<(int rowCount, IEnumerable<TrustIP> TrustIPList)> GetTrustIPs(TrustIPPara para);
        Task<TrustIP> GetTrustIPDetail(string IPBegin, string IPEnd, string cultureID);
        Task<bool> GetValidTrustIPRepeated(TrustIP trustIP);
        Task<string> EditTrustIP(TrustIP trustIP);
        Task<string> EditASPMism98(TrustIP trustIP);
        Task<string> DeleteTrustIP(string IPBegin, string IPEnd);
        Task<string> DeleteASPMism98(string IPBegin, string IPEnd);
        Task InsertTrustIPMongoDB(RecordSysTrustIP para);
    }
}