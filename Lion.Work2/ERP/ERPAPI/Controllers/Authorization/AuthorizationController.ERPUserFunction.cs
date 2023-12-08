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
        public IHttpActionResult ERPUserFunctionEditEvent([FromUri]ERPUserFunctionModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Text(string.Empty);
            }

            string apiNo = GetApiNo();

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<ERPUserFunctionModel.APIParaData>(model.APIPara);

                if (model.EditUserFunction())
                {
                    bool generateUserMenuXMLResult = GenerateUserMenuXMLResult(model.APIData.UserID);

                    model.GetERPRecordUserFunctionResult(model.APIData.UserID, apiNo, Common.GetEnumDesc(EnumLogWriter.ERPAuthorization), model.ClientSysID, ClientIPAddress());

                    if (generateUserMenuXMLResult)
                    {
                        return Text(bool.TrueString);
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