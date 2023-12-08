using System;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using ERPAPI.Models.EDIService;
using LionTech.Utility;

namespace ERPAPI.Controllers
{
    public partial class EDIServiceController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult FlowManagerExecuteEvent([FromUri]FlowManagerModel model)
        {
            if (AuthState.IsAuthorized == false) return Text(string.Empty);

            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.FlowData = jsonConvert.Deserialize<FlowManagerModel.FlowParaData>(model.FlowPara);

                string ediNo = model.ExecuteEDIFlow();
                if (ediNo != null)
                {
                    string filePath = this.GetFilePathFolderPath(
                        EnumFilePathFolder.EDIServiceFlowPara,
                        new string[] { ediNo.Substring(0, 8), ediNo });
                    Common.FileWriteStream(filePath, model.FlowPara);
                    
                    return Text(ediNo);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return Text(string.Empty);
        }

        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult FlowManagerSelectEvent([FromUri]FlowManagerModel model)
        {
            if (AuthState.IsAuthorized == false) return Text(string.Empty);
            
            try
            {
                JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
                model.FlowData = jsonConvert.Deserialize<FlowManagerModel.FlowParaData>(model.FlowPara);

                if (string.IsNullOrWhiteSpace(model.FlowData.EDINo) ||
                    string.IsNullOrWhiteSpace(model.FlowData.EDIFlowID))
                {
                    return BadRequest();
                }

                string apiParaJsonString = model.SelectEDIFlow().SerializeToJson();

                if (apiParaJsonString != null)
                {
                    return Text(apiParaJsonString);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return Text(string.Empty);
        }
    }
}
