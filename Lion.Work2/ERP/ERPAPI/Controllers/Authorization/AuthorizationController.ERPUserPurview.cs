// 新增日期：2016-11-08
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
        public IHttpActionResult ERPUserPurviewEditEvent([FromUri]ERPUserPurviewModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Text(string.Empty);
            }

            string apiNo = GetApiNo();

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<ERPUserPurviewModel.APIParaData>(model.APIPara);
                string upUser = Common.GetEnumDesc(EnumLogWriter.ERPAuthorization);
                
                if (model.EditUserPurview())
                {
                    model.RecordUserPurview(apiNo, upUser, model.ClientSysID, ClientIPAddress());
                }
                
                return Text(bool.TrueString);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return Text(bool.FalseString);
        }
    }
}