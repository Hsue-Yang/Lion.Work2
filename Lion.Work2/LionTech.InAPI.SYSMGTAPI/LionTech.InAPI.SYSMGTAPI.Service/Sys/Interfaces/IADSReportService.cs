using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface IADSReportService
    {
        Task<string> GetADSReportsCsv(string reportType, string sysID);
    }
}
