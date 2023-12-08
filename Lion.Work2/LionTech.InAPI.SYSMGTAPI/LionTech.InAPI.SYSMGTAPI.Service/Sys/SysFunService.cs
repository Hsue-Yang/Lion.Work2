using LionTech.AspNetCore.InApi.HostedService;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Service.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public partial class SysFunService : ISysFunService
    {
        private readonly ISysFunRepository _sysFunRepository;
        private readonly ISystemLogger _systemLogger;
        private readonly ISysSettingRepository _sysSettingRepository;
        private readonly ISysFunGroupRepository _sysFunGroupRepository;
        private readonly ISysRoleRepository _sysRoleRepository;
        private readonly IEventDistributorQueues _eventDistributorQueues;


        public SysFunService(ISysFunRepository sysFunRepository, ISystemLogger systemLogger, ISysSettingRepository sysSettingRepository, ISysFunGroupRepository sysFunGroupRepository, ISysRoleRepository sysRoleRepository, IEventDistributorQueues eventDistributorQueues)
        {
            _sysFunRepository = sysFunRepository;
            _systemLogger = systemLogger;
            _sysSettingRepository = sysSettingRepository;
            _sysFunGroupRepository = sysFunGroupRepository;
            _sysRoleRepository = sysRoleRepository;
            _eventDistributorQueues = eventDistributorQueues;
        }

        public Task<IEnumerable<SystemFun>> GetUserSystemFunList(string userID, string cultureID)
        {
            return _sysFunRepository.GetUserSystemFunList(userID, cultureID);
        }

        public Task<(int rowCount, IEnumerable<SystemFun> SystemFunList)> GetSystemFunList(string sysID, string subSysID, string funControllerID, string funActionName, string funGroupNM, string funNM, string funMenuSysID, string funMenu, string cultureID, int pageIndex, int pageSize)
        {
            return _sysFunRepository.GetSystemFunList(sysID, subSysID, funControllerID, funActionName, funGroupNM, funNM, funMenuSysID, funMenu, cultureID, pageIndex, pageSize);
        }

        public Task<SystemFunMain> GetSystemFunDetail(string sysID, string funControllerID, string funActionName, string cultureID)
        {
            return _sysFunRepository.GetSystemFunDetail(sysID, funControllerID, funActionName, cultureID);
        }

        public Task<IEnumerable<SystemRoleFun>> GetSystemFunRoleList(string sysID, string funControllerID, string funActionName, string cultureID)
        {
            return _sysFunRepository.GetSystemFunRoleList(sysID, funControllerID, funActionName, cultureID);
        }

        public async Task EditSystemFunByPurview(SystemFunMain systemFun)
        {
            await _sysFunRepository.EditSystemFunByPurview(systemFun);
            var systemFunDetail = await _sysFunRepository.GetSystemFunDetail(systemFun.SysID, systemFun.FunControllerID, systemFun.FunActionName, "ZH_TW");
            var systemFunRoles = await _sysFunRepository.GetSystemFunRoleList(systemFun.SysID, systemFun.FunControllerID, systemFun.FunActionName, "ZH_TW");
            var eventParaSystemFunEdit = new
            {
                systemFunDetail.SysID,
                systemFunDetail.PurviewID,
                systemFunDetail.FunControllerID,
                systemFunDetail.FunActionName,
                FunNMzhTW = systemFunDetail.FunNMZHTW,
                FunNMzhCN = systemFunDetail.FunNMZHCN,
                FunNMenUS = systemFunDetail.FunNMENUS,
                FunNMthTH = systemFunDetail.FunNMTHTH,
                FunNMjaJP = systemFunDetail.FunNMJAJP,
                FunNMkoKR = systemFunDetail.FunNMKOKR,
                systemFunDetail.IsOutside,
                systemFunDetail.IsDisable,
                systemFunDetail.SortOrder,
                RoleIDList = new List<string>()
            };

            if (systemFunRoles != null && systemFunRoles.Any())
            {
                foreach (var role in systemFunRoles)
                {
                    if (role.HasRole == EnumYN.Y.ToString())
                    {
                        eventParaSystemFunEdit.RoleIDList.Add(role.RoleID);
                    }
                }
            }

            await _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemFun, EnumEDIServiceEventID.Edit, eventParaSystemFunEdit, systemFun.SysID);
        }

        public async Task EditSystemFunDetail(DataTable systemFun, DataTable systemFunRoles, DataTable systemMenuFuns)
        {
            await _sysFunRepository.EditSystemFunDetail(systemFun, systemFunRoles, systemMenuFuns);
            var eventSystemFun = new
            {
                SysID = (string)null,
                SubSysID = (string)null,
                PurviewID = (string)null,
                FunControllerID = (string)null,
                FunActionName = (string)null,
                FunNMzhTW = (string)null,
                FunNMzhCN = (string)null,
                FunNMenUS = (string)null,
                FunNMthTH = (string)null,
                FunNMjaJP = (string)null,
                FunNMkoKR = (string)null,
                IsOutside = (string)null,
                IsDisable = (string)null,
                SortOrder = (string)null,
                RoleIDList = new List<string>()
            };
            var systemMenuFun = new
            {
                FunMenuSysID = (string)null,
                FunMenu = (string)null,
                FunMenuXAxis = (string)null,
                FunMenuYAxis = (string)null,
            };
            
            eventSystemFun = ConvertToList(systemFun, eventSystemFun).SingleOrDefault();
            var eventRoles = ConvertToList(systemFunRoles, new { RoleID = (string)null }).Select(s => s.RoleID).ToList();
            eventSystemFun = Extend(eventSystemFun, new { RoleIDList = eventRoles });
            
            var sysMenuFuns = ConvertToList(systemMenuFuns, systemMenuFun);
            var eventSystemFunMenu = new
            {
                eventSystemFun.SysID,
                eventSystemFun.FunControllerID,
                eventSystemFun.FunActionName,
                FunMenuList = new List<string>()
            };

            if (sysMenuFuns != null && sysMenuFuns.Any())
            {
                foreach (var funMenu in sysMenuFuns)
                {
                    eventSystemFunMenu.FunMenuList.Add($"{funMenu.FunMenuSysID}|{funMenu.FunMenu}|{funMenu.FunMenuXAxis}|{funMenu.FunMenuYAxis}");
                }
            }

            var systemFunMain = await _sysFunRepository.GetSystemFunDetail(eventSystemFun.SysID, eventSystemFun.FunControllerID, eventSystemFun.FunActionName, "ZH_TW");
            var eventSysFunTask =  _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemFun, EnumEDIServiceEventID.Edit, eventSystemFun, eventSystemFun.SysID);
            var eventSysFunMenuTask = _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemFunMenu, EnumEDIServiceEventID.Edit, eventSystemFunMenu, eventSystemFun.SysID);
            var logSystemRoleFunTask = RecordSystemRoleFunLog(eventSystemFun.SysID, eventSystemFun.FunControllerID,
                eventSystemFun.FunActionName, eventSystemFun.FunNMzhTW, eventSystemFun.RoleIDList, EnumSystemLogModify.U);
            var logSystemFunTask = RecordSystemFunLog(systemFunMain, EnumSystemLogModify.U);
            await Task.WhenAll(eventSysFunTask, eventSysFunMenuTask, logSystemRoleFunTask, logSystemFunTask);
        }

        private TTaget Extend<TTaget, TSource>(TTaget target, TSource source)
        {
            var targetProperties = typeof(TTaget).GetProperties();
            var sourceProperties = typeof(TSource).GetProperties();
            var targetValues = new object[targetProperties.Length];

            int index = 0;
            foreach (var targetPropertyInfo in targetProperties)
            {
                if (sourceProperties.Any(a =>
                        string.Equals(a.Name, targetPropertyInfo.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    var sourcePropertyInfo = sourceProperties
                        .SingleOrDefault(w =>
                            string.Equals(w.Name, targetPropertyInfo.Name, StringComparison.CurrentCultureIgnoreCase));
                    if (sourcePropertyInfo != null) targetValues[index] = sourcePropertyInfo.GetValue(source);
                }
                else
                {
                    targetValues[index] = targetPropertyInfo.GetValue(target);
                }

                index++;
            }

            var constructor =
                typeof(TTaget)
                    .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .OrderBy(c => c.GetParameters().Length)
                    .First();
            return (TTaget)constructor.Invoke(targetValues);
        }

        private static List<T> ConvertToList<T>(DataTable dt, T anonymousType)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var values = new object[properties.Length];
                int index = 0;
                foreach (var property in properties)
                {
                    if (columnNames.Contains(property.Name.ToLower()))
                    {
                        if (row[property.Name].GetType() != typeof(DBNull))
                        {
                            values[index] = row[property.Name];
                        }
                        index++;
                    }
                }
                var constructor =
                typeof(T)
                .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .OrderBy(c => c.GetParameters().Length)
                .First();
                return (T)constructor.Invoke(values);
            }).ToList();
        }

        public async Task<EnumDeleteSystemFunDetailResult> DeleteSystemFunDetail(string sysID, string funControllerID, string funActionName)
        {
            var systemFunMainTask = _sysFunRepository.GetSystemFunDetail(sysID, funControllerID, funActionName, "ZH_TW");
            var systemFunRoleListTask = _sysFunRepository. GetSystemFunRoleList(sysID, funControllerID, funActionName, "ZH_TW");
            await Task.WhenAll(systemFunMainTask, systemFunRoleListTask);
            
            var deleteSystemFunDetailResult = await _sysFunRepository.DeleteSystemFunDetail(sysID, funControllerID, funActionName);
            
            if (deleteSystemFunDetailResult == EnumDeleteSystemFunDetailResult.Success)
            {
                var eventSystemFun = new { SysID = sysID, FunControllerID = funControllerID, FunActionName = funActionName };
                var eventDistributorTask = _eventDistributorQueues.ProducerAsync(
                    EnumEDIServiceEventGroupID.SysSystemFun, EnumEDIServiceEventID.Delete, eventSystemFun, sysID);
                
                var logSystemRoleFunTask = RecordSystemRoleFunLog(eventSystemFun.SysID, eventSystemFun.FunControllerID,
                    eventSystemFun.FunActionName, systemFunMainTask.Result.FunNMZHTW,
                    systemFunRoleListTask.Result.Select(s => s.RoleID).ToList(), EnumSystemLogModify.D);
                var logTaskSysFun = RecordSystemFunLog(systemFunMainTask.Result, EnumSystemLogModify.D);
                
                await Task.WhenAll(eventDistributorTask, logSystemRoleFunTask, logTaskSysFun);
            }

            return deleteSystemFunDetailResult;
        }

        public Task<IEnumerable<SystemMenuFun>> GetSystemMenuFunList(string sysID, string funControllerID, string funActionName)
        {
            return _sysFunRepository.GetSystemMenuFunList(sysID, funControllerID, funActionName);
        }

        public Task<IEnumerable<SystemFunToolFunName>> GetSystemFunNameList(string sysID, string funControllerID, string cultureID)
        {
            return _sysFunRepository.GetSystemFunNameList(sysID, funControllerID, cultureID);
        }

        public Task<IEnumerable<SystemFunAction>> GetSystemFunActionList(string cultureID)
        {
            return _sysFunRepository.GetSystemFunActionList(cultureID);
        }

        private async Task RecordSystemFunLog(SystemFunMain model, EnumSystemLogModify modifyType)
        {
            if (model.SysID == model.SubSysID)
            {
                model.SubSysNM = model.SysNM;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.SubSysID))
                {
                    var systemMain = await _sysSettingRepository.GetSystemMain(model.SubSysID);
                    model.SubSysNM = systemMain.SysNMZHTW;
                }
            }

            await _systemLogger.RecordLogAsync(EnumMongoDocName.LOG_SYS_SYSTEM_FUN, model,
                model => new { model.SysID, model.FunControllerID, model.FunActionName }, modifyType);
        }

        private async Task RecordSystemRoleFunLog(string sysID, string funControllerID, string funActionName, string funNM, List<string> roles, EnumSystemLogModify modifyType)
        {
            var systemMainTask = _sysSettingRepository.GetSystemMain(sysID);
            var systemFunGroupDetailTask = _sysFunGroupRepository.GetSystemFunGroupDetail(sysID, funControllerID);
            var systemRoleByIdTask = _sysRoleRepository.GetSystemRoleByIdList(sysID, null, "ZH_TW");
            await Task.WhenAll(systemMainTask, systemFunGroupDetailTask, systemRoleByIdTask);

            var log =
                (from role in roles
                    join id in systemRoleByIdTask.Result.ToList()
                        on role equals id.RoleID
                    select new
                    {
                        SYS_ID = sysID,
                        SYS_NM = systemMainTask.Result.SysNMZHTW,
                        ROLE_ID = role,
                        ROLE_NM = id.RoleNM,
                        FUN_CONTROLLER_ID = funControllerID,
                        FUN_GROUP_NM = systemFunGroupDetailTask.Result.FunGroupZHTW,
                        FUN_ACTION_NAME = funActionName,
                        FUN_NM = funNM
                    }).ToList();

            await _systemLogger.RecordLogAsync(EnumMongoDocName.LOG_SYS_SYSTEM_ROLE_FUN, log,
                model => new { model.SYS_ID, model.FUN_CONTROLLER_ID, model.FUN_ACTION_NAME }, modifyType);
        }
    }
}