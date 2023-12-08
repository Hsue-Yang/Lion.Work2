using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Service.Utility.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysFunElmService : ISysFunElmService
    {
        private readonly ISysFunElmRepository _systemFunElmRepository;
        private readonly ISystemLogger _systemLogger;
        private readonly ISysSettingRepository _sysSettingRepository;
        public SysFunElmService(ISysFunElmRepository systemFunElmRepository, ISystemLogger systemLogger, ISysSettingRepository sysSettingRepository)
        {
            _systemFunElmRepository = systemFunElmRepository;
            _systemLogger = systemLogger;
            _sysSettingRepository = sysSettingRepository;
        }

        public Task<(int rowCount, IEnumerable<SystemFunElm> systemFunElms)> GetSysFunElmList(string sysID, string isDisable, string elmID, string elmName, string funControllerID, string funActionName, string cultureID, int pageIndex, int pageSize)
        {
            return _systemFunElmRepository.GetSysFunElmList(sysID, isDisable, elmID, elmName, funControllerID, funActionName, cultureID, pageIndex, pageSize);
        }

        public Task<bool> CheckSystemFunElmIdIsExists(string sysID, string elmID, string funControllerID, string funActionName)
        {
            return _systemFunElmRepository.CheckSystemFunElmIdIsExists(sysID, elmID, funControllerID, funActionName);
        }

        public Task<SystemFunElm> GetSystemFunElmDetail(string sysID, string elmID, string funControllerID, string funActionName)
        {
            return _systemFunElmRepository.GetSystemFunElmDetail(sysID, elmID, funControllerID, funActionName);
        }

        public Task<IEnumerable<SystemRoleFunElm>> GetSystemFunElmRoleList(string sysID, string elmID, string funControllerID, string funActionName, string cultureID)
        {
            return _systemFunElmRepository.GetSystemFunElmRoleList(sysID, elmID, funControllerID, funActionName, cultureID);
        }

        public Task<SystemFunElm> GetSystemFunElmInfo(string sysID, string elmID, string funControllerID, string funActionName, string cultureID)
        {
            return _systemFunElmRepository.GetSystemFunElmInfo(sysID, elmID, funControllerID, funActionName, cultureID);
        }

        public async Task EditSystemFunElmDetail(SystemFunElm systemFunElm)
        {
            await _systemFunElmRepository.EditSystemFunElmDetail(systemFunElm);
            await RecordSystemFunElmLog(systemFunElm);
        }

        public async Task EditSystemFunElmRole(string sysID, string elmID, string funControllerID, string funActionName, List<ElmRoleInfoValue> elmRoleInfoValues)
        {
            await _systemFunElmRepository.EditSystemFunElmRole(sysID, elmID, funControllerID, funActionName, elmRoleInfoValues);
            await RecordSystemRoleFunElmLog(sysID, elmID, funControllerID, funActionName, elmRoleInfoValues);
        }

        private async Task RecordSystemFunElmLog(SystemFunElm systemFunElm)
        {
            var systemMainTask = _sysSettingRepository.GetSystemMain(systemFunElm.SysID);
            var cmCodesTask = _sysSettingRepository.GetCodeManagementList("0045", "ZH_TW", null, null);
            await Task.WhenAll(systemMainTask, systemMainTask);

            var systemMain = systemMainTask.Result;
            var cmCodes = cmCodesTask.Result;
            
            systemFunElm.SysNM = systemMain.SysNMZHTW;
            
            var funElmDisplayTypeDic =
                (from s in cmCodes
                    select new
                    {
                        s.CodeID,
                        s.CodeNM
                    }).ToDictionary(k => k.CodeID, v => v.CodeNM);
            
            systemFunElm.DefaultDisplay = funElmDisplayTypeDic[systemFunElm.DefaultDisplaySts.ToString()];
            await _systemLogger.RecordLogAsync(EnumMongoDocName.LOG_SYS_SYSTEM_FUN_ELM, systemFunElm, model => new { model.SysID, model.ElmID }, EnumSystemLogModify.U);
        }

        private async Task RecordSystemRoleFunElmLog(string sysID, string elmID, string funControllerID, string funActionName, List<ElmRoleInfoValue> elmRoleInfoValues)
        {
            var systemMainTask = _sysSettingRepository.GetSystemMain(sysID);
            var cmCodesTask = _sysSettingRepository.GetCodeManagementList("0045", "ZH_TW", null, null);
            await Task.WhenAll(systemMainTask, systemMainTask);

            var systemMain = systemMainTask.Result;
            var cmCodes = cmCodesTask.Result;
            
            var funElmDisplayTypeDic =
                (from s in cmCodes
                    select new
                    {
                        s.CodeID,
                        s.CodeNM
                    }).ToDictionary(k => k.CodeID, v => v.CodeNM);

            var log = new
            {
                SYS_ID = sysID,
                SYS_NM = systemMain.SysNMZHTW,
                ELM_ID = elmID,
                CONTROLLER_NAME = funControllerID,
                ACTION_NAME = funActionName,
                ELM_ROLE_LIST =
                    (from s in elmRoleInfoValues
                        group s by new { s.DISPLAY_STS }
                        into g
                        select new
                        {
                            g.Key.DISPLAY_STS,
                            DISPLAY_NM = funElmDisplayTypeDic[g.Key.DISPLAY_STS.ToString()],
                            ROLE_ID_LIST = g.Select(sm => sm.ROLE_ID).ToList()
                        }).ToList()
            };
            await _systemLogger.RecordLogAsync(EnumMongoDocName.LOG_SYS_SYSTEM_ROLE_FUN_ELM, log, model => new { }, EnumSystemLogModify.U);
        }
    }
}