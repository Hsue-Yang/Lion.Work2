using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Mis.Interfaces
{
    public interface IMisService
    {
        Task<string> CheckIP(string UserCode, int userIP);
    }
}
