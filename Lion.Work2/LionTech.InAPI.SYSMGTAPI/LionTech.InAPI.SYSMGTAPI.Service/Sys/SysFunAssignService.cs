using System;
using LionTech.AspNetCore.InApi.HostedService;
using LionTech.InAPI.SYSMGTAPI.DataAccess.Sys.Interfaces;
using LionTech.InAPI.SYSMGTAPI.Domain.Entities.Sys;
using LionTech.InAPI.SYSMGTAPI.Service.Sys.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LionTech.InAPI.SYSMGTAPI.Service.Sys
{
    public class SysFunAssignService : ISysFunAssignService
    {
        private readonly ISysFunAssignRepository _sysFunAssignRepository;
        private readonly IEventDistributorQueues _eventDistributorQueues;
        private readonly IUserFunctionRepository _userFunctionRepository;
        private readonly IUserFunctionService _userFunctionService;

        public SysFunAssignService(ISysFunAssignRepository sysFunAssignRepository,
                                   IEventDistributorQueues eventDistributorQueues,
                                   IUserFunctionRepository userFunctionRepository,
                                   IUserFunctionService userFunctionService)
        {
            _sysFunAssignRepository = sysFunAssignRepository;
            _eventDistributorQueues = eventDistributorQueues;
            _userFunctionRepository = userFunctionRepository;
            _userFunctionService = userFunctionService;
        }

        public async Task<IEnumerable<SystemFunAssign>> GetSystemFunAssigns(string sysID, string funControllerID, string funActionName)
        {
            return await _sysFunAssignRepository.GetSystemFunAssigns(sysID, funControllerID, funActionName);
        }

        public async Task EditSystemFunAssign(SystemFunAssignPara para)
        {
            var userList = await GetUserList(para);
            var originUserFunctions = await GetUserFunctions(userList.Select(s => s.UserID));
            
            var systemFunAssignUsers = para.UserIDList.Select(x => new SystemFunAssignUser { UserID = x }).ToList();
            await _sysFunAssignRepository.EditSystemFunAssign(para.SysID, para.FunControllerID, para.FunActionName, para.UpdUserID, systemFunAssignUsers);
            var eventPara = new
            {
                para.SysID,
                para.FunControllerID,
                para.FunActionName,
                para.UserIDList
            };

            await _eventDistributorQueues.ProducerAsync(EnumEDIServiceEventGroupID.SysSystemFunAssign, EnumEDIServiceEventID.Edit, eventPara, para.SysID);
            
            foreach (var user in userList)
            {
                var userFunctions = new List<UserFunctionValue>();
                if (para.UserIDList.Exists(e => e == user.UserID) == false)
                {
                    userFunctions.Add(new()
                    {
                        SysID = string.Empty, 
                        FunControllerID = string.Empty, 
                        FunActionName = string.Empty
                    });
                }
                else
                {
                    userFunctions.Add( new()
                    {
                        SysID = para.SysID,
                        FunControllerID = para.FunControllerID,
                        FunActionName = para.FunActionName,
                    });
                }

                var originUserFunction =
                    originUserFunctions.Where(w => w.UserID == user.UserID && w.SysID == para.SysID &&
                                                   w.FunControllerID == para.FunControllerID &&
                                                   w.FunActionName == para.FunActionName);
                var logTaskUserFunApply = _userFunctionService.RecordUserFunApply(user.UserID, user.UserNM,
                    para.ErpWFNO, para.Memo, userFunctions, originUserFunction);
                var logTaskUserFun = _userFunctionService.RecordUserFunction(user.UserID, user.UserNM, para.ErpWFNO, para.Memo);
                await Task.WhenAll(logTaskUserFunApply, logTaskUserFun);
            }
        }

        private async Task<IEnumerable<UserMain>> GetUserList(SystemFunAssignPara para)
        {
            var userList =
                (await _sysFunAssignRepository.GetSystemFunAssigns(para.SysID, para.FunControllerID,
                    para.FunActionName)).Select(s => new { s.UserID, s.UserNM });

            var userRawDatas = await _sysFunAssignRepository.GetUserRawDatas(para.UserIDList
                .Select(x => new UserMain { UserID = x }).ToList());

            userList = userList.Concat(userRawDatas.Select(s => new
            {
                s.UserID, UserNM = s.UserNM.Replace(s.UserID, string.Empty, StringComparison.OrdinalIgnoreCase).Trim()
            })).Distinct();

            return userList.Select(s => new UserMain { UserID = s.UserID, UserNM = s.UserNM });
        }

        public Task<IEnumerable<SysFunRawData>> GetFunRawDatas(List<UserFunctionValue> userFunction, string userID, string cultureID)
        {
            return _sysFunAssignRepository.GetFunRawDatas(userFunction, userID, cultureID);
        }

        public Task<IEnumerable<UserMain>> GetUserRawDatas(List<UserMain> userIDList)
        {
            return _sysFunAssignRepository.GetUserRawDatas(userIDList);
        }

        private async Task<IEnumerable<UserFunction>> GetUserFunctions(IEnumerable<string> userList)
        {
            var result = new List<UserFunction>();
            foreach (var user in userList)
            {
                var userFunctions = await _userFunctionRepository.GetUserFunctions(user, user, "ZH_TW");
                foreach (var userFunction in userFunctions)
                {
                    userFunction.UserID = user;
                }
                result.AddRange(userFunctions);
            }

            return result;
        }
    }
}