﻿// 新增日期：2018-06-20
// 新增人員：方道筌
// 新增內容：
// ---------------------------------------------------

using ERPAPI.Models.SystemSetting;
using LionTech.APIService.SystemSetting;
using LionTech.Utility;
using System;
using System.Text;
using System.Web;
using System.Web.Http;

namespace ERPAPI.Controllers
{
    public partial class SystemSettingController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public IHttpActionResult ERPFunElmRoleEditEvent([FromUri]SystemFunElmRoleModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Unauthorized();
            }

            string jsonPara = Common.GetStreamToString(MSHttpContext.Request.InputStream, Encoding.UTF8);
            
            try
            {
                string updUserID = string.IsNullOrWhiteSpace(model.ClientUserID) ? Common.GetEnumDesc(EnumLogWriter.ERPAuthorization) : model.ClientUserID;

                SystemFunElmRoleItem funElmRoleItem = Common.GetJsonDeserializeObject<SystemFunElmRoleItem>(jsonPara);

                if (model.EditSystemFunElmRole(funElmRoleItem, updUserID))
                {
                    return Text(bool.TrueString);
                }
                
                return Text(bool.FalseString);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return InternalServerError();
        }
    }
}