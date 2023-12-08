using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using ERPAP.Models.Sys;
using LionTech.Entity.ERP;
using LionTech.Utility;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        public enum FilePathKeyWord
        {
            FileData, LionTech, EDIService
        }

        [HttpPost]
        [AuthorizationActionFilter]
        public async Task<ActionResult> SystemEDIFlowError(SystemEDIFlowErrorModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            model.EDIFilePath = string.Format(Common.GetEnumDesc(EnumEDIServicePathFile.EDIServiceSubscriberLog), model.EDIFlowID, model.EDIDate, model.EDITime);

            string fullFilePath = Path.Combine(
                new string[]
                {

                    await model.GetSysSystemFilePath(model.QuerySysID, AuthState.SessionData.UserID),
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