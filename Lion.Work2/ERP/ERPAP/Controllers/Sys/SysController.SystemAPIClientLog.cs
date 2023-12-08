using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Utility;
using System.IO;
using LionTech.Entity.ERP;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemAPIClientLog(SystemAPIClientLogModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.EDIFilePath = string.Format(Common.GetEnumDesc(EnumEDIServicePathFile.EDIServiceSubscriberLog), SystemEventTargetLogModel.EDIFlowID.SUBS.ToString(), model.EDIDate, model.EDITime);
            string fullFilePath = Path.Combine(
                new string[]
                    {
                        model.GetFileDataPath(base.CultureID),
                        SysModel.EnumFilePathKeyWord.FileData.ToString(),
                        Common.GetEnumDesc(EnumEDISystemID.ERPAP),
                        model.EDIFilePath
                    });
            model.EDIFile = Common.FileReadStream(fullFilePath);
            
            return View(model);
        }
    }
}