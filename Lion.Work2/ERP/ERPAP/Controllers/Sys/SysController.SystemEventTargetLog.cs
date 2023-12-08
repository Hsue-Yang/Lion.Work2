using ERPAP.Models.Sys;
using LionTech.Entity.ERP;
using LionTech.Utility;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemEventTargetLog(SystemEventTargetLogModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.EDIFilePath = string.Format(Common.GetEnumDesc(EnumEDIServicePathFile.EDIServiceSubscriberLog), SystemEventTargetLogModel.EDIFlowID.SUBS.ToString(), model.EDIDate, model.EDITime);
            string fullFilePath = Path.Combine(
                new string[]
                    {
                        await model.GetSysSystemFilePath(EnumSystemID.ERPAP.ToString(), AuthState.SessionData.UserID),
                        SysModel.EnumFilePathKeyWord.FileData.ToString(),
                        Common.GetEnumDesc(EnumEDISystemID.ERPAP),
                        model.EDIFilePath
                    });
            model.EDIFile = Common.FileReadStream(fullFilePath);

            return View(model);
        }
    }
}