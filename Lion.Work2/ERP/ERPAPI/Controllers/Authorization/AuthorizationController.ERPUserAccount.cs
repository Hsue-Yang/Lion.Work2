// 新增日期：2016-09-19
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
        public IHttpActionResult ERPUserAccountCreateEvent([FromUri]ERPUserModel model)
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

                if (model.CreateUserAccount(upUser))
                {
                    model.RecordCreateUserAccount(apiNo, upUser, model.ClientSysID, ClientIPAddress());

                    if (model.EditSysUserRoleList(upUser))
                    {
                        model.RecordSysUserRole(apiNo, upUser, model.ClientSysID, ClientIPAddress());

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