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
        public IHttpActionResult ERPFunAssignEditEvent([FromUri]ERPFunAssignModel model)
        {
            if (AuthState.IsAuthorized == false) return Text(string.Empty);
            string apiNo = GetApiNo();

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<ERPFunAssignModel.APIParaData>(model.APIPara);

                if (model.EditUserFun())
                {
                    foreach (string userID in model.APIData.UserIDList)
                    {
                        model.GetERPRecordUserFunctionResult(userID, apiNo, Common.GetEnumDesc(EnumLogWriter.ERPAuthorization), model.ClientSysID, ClientIPAddress());
                    }

                    return Text(bool.TrueString);
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