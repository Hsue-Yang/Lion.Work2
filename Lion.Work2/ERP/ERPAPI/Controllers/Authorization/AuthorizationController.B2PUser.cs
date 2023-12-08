using System;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using ERPAPI.Models.Authorization;
using LionTech.Entity.B2P;

namespace ERPAPI.Controllers
{
    public partial class AuthorizationController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult B2PUserCreateEvent([FromUri]B2PUserModel model)
        {
            if (AuthState.IsAuthorized == false) return Text(string.Empty);
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<B2PUserModel.APIParaData>(model.APIPara);

                if (model.CreateB2PUserAccount())
                {
                    base.ExecB2PUserRolePLogWrite(model.APIData.UserID, EnumLogWriter.B2PUpdUserID, model.ClientSysID);

                    string filePath = base.GetB2PAPUserMenuFilePath(model.APIData.UserID, EnumCultureID.zh_TW);
                    bool generateUserMenuXMLResult = model.GenerateUserMenuXML(model.APIData.UserID, filePath, EnumCultureID.zh_TW);

                    if (generateUserMenuXMLResult)
                    {
                        filePath = base.GetB2PAPUserMenuFilePath(model.APIData.UserID, EnumCultureID.zh_CN);
                        generateUserMenuXMLResult = model.GenerateUserMenuXML(model.APIData.UserID, filePath, EnumCultureID.zh_CN);
                    }
                    if (generateUserMenuXMLResult)
                    {
                        filePath = base.GetB2PAPUserMenuFilePath(model.APIData.UserID, EnumCultureID.en_US);
                        generateUserMenuXMLResult = model.GenerateUserMenuXML(model.APIData.UserID, filePath, EnumCultureID.en_US);
                    }
                    if (generateUserMenuXMLResult)
                    {
                        filePath = base.GetB2PAPUserMenuFilePath(model.APIData.UserID, EnumCultureID.th_TH);
                        generateUserMenuXMLResult = model.GenerateUserMenuXML(model.APIData.UserID, filePath, EnumCultureID.th_TH);
                    }
                    if (generateUserMenuXMLResult)
                    {
                        filePath = base.GetB2PAPUserMenuFilePath(model.APIData.UserID, EnumCultureID.ja_JP);
                        generateUserMenuXMLResult = model.GenerateUserMenuXML(model.APIData.UserID, filePath, EnumCultureID.ja_JP);
                    }

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

        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult B2PUserDeleteEvent([FromUri]B2PUserModel model)
        {
            if (AuthState.IsAuthorized == false) return Text(string.Empty);
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<B2PUserModel.APIParaData>(model.APIPara);

                if (model.DeleteB2PUserAccount())
                {
                    base.ExecB2PUserRolePLogWrite(model.APIData.UserID, EnumLogWriter.B2PUpdUserID, model.ClientSysID);
                    
                    return Text(bool.TrueString);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return Text(bool.FalseString);
        }

        [HttpGet]
        [AuthorizationActionFilter]
        public string B2PUserCheckExistEvent([FromUri]B2PUserModel model)
        {
            if (AuthState.IsAuthorized == false) return string.Empty;

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.APIData = jsonConvert.Deserialize<B2PUserModel.APIParaData>(model.APIPara);

                if (model.CheckB2PUserAccountIsExist())
                {
                    return bool.TrueString;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return bool.FalseString;
        }
    }
}