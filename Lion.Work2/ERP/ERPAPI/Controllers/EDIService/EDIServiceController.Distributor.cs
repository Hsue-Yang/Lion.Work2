using ERPAPI.Models.EDIService;
using LionTech.Utility;
using System;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ERPAPI.Controllers
{
    public partial class EDIServiceController
    {
        [HttpGet]
        [AuthorizationActionFilter]
        public IHttpActionResult Distributor([FromUri]DistributorModel model)
        {
            if (AuthState.IsAuthorized == false)
            {
                return Ok();
            }

            try
            {
                string ediEventNo = model.ExcuteSubscription();
                if (string.IsNullOrWhiteSpace(ediEventNo) == false)
                {
                    string filePath = GetFilePathFolderPath(
                        EnumFilePathFolder.EDIServiceEventPara,
                        new[] { ediEventNo.Substring(0, 8), ediEventNo });
                    int reTryCount = 0;

                    do
                    {
                        Common.FileWriteStream(filePath, model.EventPara);
                        if (System.IO.File.Exists(filePath))
                        {
                            break;
                        }
                        reTryCount++;
                    } while (reTryCount < 10);

                    return Content(HttpStatusCode.OK, ediEventNo, new PlainTextFormatter());
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            
            return Ok();
        }
    }
}