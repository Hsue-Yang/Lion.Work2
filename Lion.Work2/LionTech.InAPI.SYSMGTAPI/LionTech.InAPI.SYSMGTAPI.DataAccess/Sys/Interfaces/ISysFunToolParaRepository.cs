using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces
{
    public interface ISysFunToolParaRepository
    {
        Task<SystemFunTool> GetSystemFunToolParaForms(string userID, string sysID, string funControllerID, string funActionName, string toolNo, string cultureID);
        Task<(int count, IEnumerable<SystemFunTool>)> GetSystemFunToolParas(string userID, string sysID, string funControllerID, string funActionName, string toolNo, int pageIndex, int pageSize);
    }
}
