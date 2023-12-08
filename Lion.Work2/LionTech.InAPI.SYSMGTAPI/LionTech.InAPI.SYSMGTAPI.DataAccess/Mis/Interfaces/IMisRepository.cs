using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Mis.Interfaces
{
    public interface IMisRepository
    {
        Task<string> CheckIP(string stfn, int ip);
    }
}
