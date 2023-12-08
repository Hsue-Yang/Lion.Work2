using LionTech.AspNetCore.InApi.HostedService;
using LionTech.AspNetCore.Utility.SERP;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Raw.Interfaces;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Domain.Enums;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Service.Utility.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LionTech.AspNetCore.Utility;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class UserFunctionService : IUserFunctionService
    {
        private readonly IUserFunctionRepository _userFunctionRepository;
        private readonly ISystemLogger _systemLogger;
        private readonly IRawRepository _rawRepository;
        private readonly ISysFunAssignRepository _sysFunAssignRepository;
        private readonly IEventDistributorQueues _eventDistributorQueues;
        private readonly ISysEventRepository _sysEventRepository;


        public UserFunctionService(
            IUserFunctionRepository userFunctionRepository,
            ISystemLogger systemLogger,
            IRawRepository rawRepository,
            ISysFunAssignRepository sysFunAssignRepository,
            IEventDistributorQueues eventDistributorQueues,
            ISysEventRepository sysEventRepository)
        {
            _userFunctionRepository = userFunctionRepository;
            _systemLogger = systemLogger;
            _rawRepository = rawRepository;
            _sysFunAssignRepository = sysFunAssignRepository;
            _eventDistributorQueues = eventDistributorQueues;
            _sysEventRepository = sysEventRepository;
        }

        public async Task<IEnumerable<UserFunction>> GetUserFunctions(string userID, string updUserID, string cultureID)
        {
            return await _userFunctionRepository.GetUserFunctions(userID, updUserID, cultureID);
        }

        public async Task EditUserFunction(UserFunctionDetail para)
        {
            IEnumerable<UserFunction> originUserFunctions = await _userFunctionRepository.GetUserFunctions(para.UserID, para.UpdUserID, "ZH_TW");
            await _userFunctionRepository.EditUserFunction(para.UserID, para.IsDisable, para.UpdUserID, para.FunctionList);
            var systemEventTargets = await _sysEventRepository.GetSystemEventTargetIDs(EnumSystemID.ERPAP.ToString(), EnumEDIServiceEventGroupID.SysUserFunction.ToString(), EnumEDIServiceEventID.Edit.ToString());
            if (systemEventTargets != null && systemEventTargets.Any())
            {
                foreach (SystemEventTarget target in systemEventTargets)
                {
                    var eventPara = new
                    {
                        para.UserID,
                        FunctionList = para.FunctionList.Select(x => $"{x.SysID}|{x.FunControllerID}|{x.FunActionName}")
                    };

                    await _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysUserFunction, EnumEDIServiceEventID.Edit, eventPara, target.SysID);
                }
                
                var rawUserTask = await _rawRepository.GetRawUsers(para.UserID, 1);
                var userNM = rawUserTask.SingleOrDefault()?.UserNM;
                var logTaskUserFunApply = RecordUserFunApply(para.UserID, userNM, para.ErpWFNO, para.Memo,
                    para.FunctionList, originUserFunctions);
                var logTaskUserFun = RecordUserFunction(para.UserID, userNM, para.ErpWFNO, para.Memo);
                await Task.WhenAll(logTaskUserFunApply, logTaskUserFun);
            }
        }

        public async Task RecordUserFunApply(string userID, string userNM, string erpWFNO, string memo,
            List<UserFunctionValue> userFunctionList,
            IEnumerable<UserFunction> originUserFunctionList)
        {
            if (userFunctionList.Any())
            {
                var sysFunRawDatas = await _sysFunAssignRepository.GetFunRawDatas(userFunctionList, userID, "ZH_TW");
                var modifyList =
                    (from s in originUserFunctionList
                        where userFunctionList.Exists(u => u.SysID == s.SysID &&
                                                           u.FunControllerID == s.FunControllerID &&
                                                           u.FunActionName == s.FunActionName) == false
                        select new
                        {
                            SYS_ID = s.SysID,
                            SYS_NM = s.SysNM,
                            FUN_CONTROLLER_ID = s.FunControllerID,
                            FUN_CONTROLLER_NM = s.FunGroupNM,
                            FUN_ACTION_NAME = s.FunActionName,
                            FUN_NM = s.FunNM,
                            MOTIFY_TYPE = EnumModifyType.D.ToString(),
                            MODIFY_TYPE_NM = Common.GetEnumDesc(EnumModifyType.D)
                        }).Concat(
                        (from f in userFunctionList
                            join sysFunRawData in sysFunRawDatas
                                on new
                                {
                                    f.SysID,
                                    f.FunControllerID,
                                    f.FunActionName
                                } equals new
                                {
                                    sysFunRawData.SysID,
                                    sysFunRawData.FunControllerID,
                                    FunActionName = sysFunRawData.FunActionID
                                }
                            where originUserFunctionList.Any(fun => fun.SysID == f.SysID &&
                                                                    fun.FunControllerID == f.FunControllerID &&
                                                                    fun.FunActionName == f.FunActionName) == false
                            select new
                            {
                                SYS_ID = f.SysID,
                                SYS_NM = sysFunRawData.SysNM,
                                FUN_CONTROLLER_ID = f.FunControllerID,
                                FUN_CONTROLLER_NM = sysFunRawData.FunControllerNM,
                                FUN_ACTION_NAME = f.FunActionName,
                                FUN_NM = sysFunRawData.FunActionNM,
                                MOTIFY_TYPE = EnumModifyType.I.ToString(),
                                MODIFY_TYPE_NM = Common.GetEnumDesc(EnumModifyType.I)
                            })
                    ).ToList();

                if (modifyList.Any())
                {
                    var log = new
                    {
                        USER_ID = userID,
                        USER_NM = userNM,
                        WFNO = erpWFNO,
                        MEMO = memo,
                        MODIFY_LIST = modifyList
                    };
                    await _systemLogger.RecordLogAsync(EnumMongoDocName.LOG_USER_FUN_APPLY, log,
                        model => new { model.USER_ID }, EnumSystemLogModify.U);
                }
            }
            else
            {
                var modifyList =
                    (from s in originUserFunctionList
                        select new
                        {
                            SYS_ID =  s.SysID,
                            SYS_NM =  s.SysNM,
                            FUN_CONTROLLER_ID = s.FunControllerID,
                            FUN_CONTROLLER_NM = s.FunGroupNM,
                            FUN_ACTION_NAME = s.FunActionName,
                            FUN_NM = s.FunNM,
                            MOTIFY_TYPE = EnumModifyType.I.ToString(),
                            MODIFY_TYPE_NM = Common.GetEnumDesc(EnumModifyType.D)
                        }).ToList();
                
                if (modifyList.Any())
                {
                    var log = new
                    {
                        USER_ID = userID,
                        USER_NM = userNM,
                        WFNO = erpWFNO,
                        MEMO = memo,
                        MODIFY_LIST = modifyList
                    };
                    await _systemLogger.RecordLogAsync(EnumMongoDocName.LOG_USER_FUN_APPLY, log,
                        model => new { model.USER_ID }, EnumSystemLogModify.U);
                }
            }
        }

        public async Task RecordUserFunction(string userID, string userNM, string erpWFNO, string memo)
        {
            var userFunctions = await _userFunctionRepository.GetAllSystemUserFunctions(userID, "ZH_TW");
            if (userFunctions.Any())
            {
                var log = userFunctions.Select(item => new
                {
                    USER_ID = userID,
                    USER_NM = userNM,
                    SYS_ID = item.SysID,
                    SYS_NM = item.SysNM,
                    FUN_CONTROLLER_ID = item.FunControllerID,
                    FUN_CONTROLLER_NM = item.FunGroupNM,
                    FUN_ACTION_NAME = item.FunActionName,
                    FUN_NM = item.FunNM,
                    WFNO = erpWFNO,
                    MEMO = memo,
                }).ToList();
                await _systemLogger.RecordLogAsync(EnumMongoDocName.LOG_USER_FUN, log, model => new { model.USER_ID }, EnumSystemLogModify.U);
            }
            else
            {
                var log =
                    new[] {
                        new {
                            USER_ID = userID,
                            USER_NM = userNM,
                            SYS_ID = (string)null,
                            SYS_NM = (string)null,
                            FUN_CONTROLLER_ID = (string)null,
                            FUN_CONTROLLER_NM = (string)null,
                            FUN_ACTION_NAME = (string)null,
                            FUN_NM = (string)null,
                            WFNO = erpWFNO,
                            MEMO = memo,
                        }
                    }.ToList();
                await _systemLogger.RecordLogAsync(EnumMongoDocName.LOG_USER_FUN, log, model => new { model.USER_ID }, EnumSystemLogModify.U);
            }
        }
    }
}