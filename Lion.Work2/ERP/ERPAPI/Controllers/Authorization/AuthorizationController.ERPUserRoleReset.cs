// 新增日期：2016-09-21
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using ERPAPI.Models.Authorization;
using LionTech.Utility;

namespace ERPAPI.Controllers
{
    public partial class AuthorizationController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPUserRoleResetEvent([FromUri]ERPUserModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Text(string.Empty);
            }

            string apiNo = GetApiNo();

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.ApiUserAccountData = jsonConvert.Deserialize<ERPUserModel.APIParaUserAccountData>(model.APIPara);
                string upUser = Common.GetEnumDesc(EnumLogWriter.ERPAuthorization);

                if (model.EditRawUserData(upUser))
                {
                    model.RecordModifyUserAccount(apiNo, upUser, model.ClientSysID, ClientIPAddress());

                    if (model.EditSysUserRoleList(upUser))
                    {
                        if (model.ApiUserAccountData.IsIncludeAuthApplyed)
                        {
                            model.EditUserSystemRoleApply(upUser);
                            model.EditUserFunApply(upUser);
                        }
                        else
                        {
                            //紀錄此次[不保留原有權限]的日期
                            model.InsertUserSystemRoleApplyBaseLineDate(apiNo, upUser, model.ClientSysID, ClientIPAddress());
                            model.InsertUserFunApplyBaseLineDate(apiNo, upUser, model.ClientSysID, ClientIPAddress());
                        }

                        model.RecordSysUserRole(apiNo, upUser, model.ClientSysID, ClientIPAddress());

                        model.GetERPRecordUserFunctionResult(model.ApiUserAccountData.UserID, apiNo, upUser, model.ClientSysID, ClientIPAddress());

                        bool generateUserMenuXMLResult = GenerateUserMenuXMLResult(model.ApiUserAccountData.UserID);

                        if (generateUserMenuXMLResult)
                        {
                            foreach (var row in model.SysRoleEventParaList)
                            {
                                ExecEDIServiceDistributor(EnumEDIServiceEventGroupID.SysUserSystemRole, EnumEDIServiceEventID.Edit, jsonConvert.Serialize(row), EnumLogWriter.ERPAuthorization.ToString());
                            }
                            
                            return Text(bool.TrueString);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
           
            return Text(bool.FalseString);
        }
    }
}