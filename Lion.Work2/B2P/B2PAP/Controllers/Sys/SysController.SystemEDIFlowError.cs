using System.IO;
using System.Web.Mvc;
using B2PAP.Models.Sys;
using LionTech.Entity.B2P;
using LionTech.Utility;

namespace B2PAP.Controllers
{
    public partial class SysController
    {
        public enum FilePathKeyWord
        {
            FileData, LionTech, EDIService
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemEDIFlowError(SystemEDIFlowErrorModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.EDIFilePath = string.Format(Common.GetEnumDesc(EnumEDIServicePathFile.EDIServiceSubscriberLog),model.EDIFlowID, model.EDIDate, model.EDITime);

            string fullFilePath = Path.Combine(
                new string[]
                {
                    model.GetFileDataPath(base.CultureID),
                    SysModel.EnumFilePathKeyWord.FileData.ToString(),
                    Common.GetEnumDesc(Utility.GetEnumEDISystemID(model.QuerySysID)),
                    model.EDIFilePath
                }
            );

            model.EDIFile = Common.FileReadStream(fullFilePath);

            return View(model);
        }
    }
}