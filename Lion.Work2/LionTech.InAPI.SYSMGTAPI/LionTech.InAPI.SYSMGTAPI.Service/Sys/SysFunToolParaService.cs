using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysFunToolParaService : ISysFunToolParaService
    {
        private readonly ISysFunToolParaRepository _sysFunToolParaRepository;

        public SysFunToolParaService(ISysFunToolParaRepository sysFunToolParaRepository)
        {
            _sysFunToolParaRepository = sysFunToolParaRepository;
        }

        public async Task<SystemFunTool> GetSystemFunToolParaForms(string userID, string sysID, string funControllerID, string funActionName, string toolNo, string cultureID)
        {
            return await _sysFunToolParaRepository.GetSystemFunToolParaForms(userID, sysID, funControllerID, funActionName, toolNo, cultureID);
        }

        public async Task<(int count,IEnumerable<SystemFunTool>)> GetSystemFunToolParas(string userID, string sysID, string funControllerID, string funActionName, string toolNo, int pageIndex, int pageSize)
        {
            return await _sysFunToolParaRepository.GetSystemFunToolParas(userID, sysID, funControllerID, funActionName, toolNo, pageIndex, pageSize);
        }
    }
}