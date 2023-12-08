using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces
{
    public interface ISysFunToolParaService
    {
        Task<SystemFunTool> GetSystemFunToolParaForms(string userID, string sysID, string funControllerID, string funActionName, string toolNo, string cultureID);
        Task<(int count,IEnumerable<SystemFunTool>)> GetSystemFunToolParas(string userID, string sysID, string funControllerID, string funActionName, string toolNo, int pageIndex, int pageSize);
    }
}
