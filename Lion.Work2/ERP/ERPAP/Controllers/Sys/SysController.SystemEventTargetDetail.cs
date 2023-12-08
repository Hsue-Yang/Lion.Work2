using ERPAP.Models.Sys;
using LionTech.Utility;
using System.Web.Mvc;

namespace ERPAP.Controllers
{
    public partial class SysController
    {
        [HttpPost]
        [AuthorizationActionFilter]
        public ActionResult SystemEventTargetDetail(SystemEventTargetDetailModel model)
        {
            if (AuthState.IsAuthorized == false) return AuthState.UnAuthorizedActionResult;

            string execEDIEventNo = model.ExecEDIEventNo;
            string eventParaFilePath = GetFilePathFolderPath(
                EnumFilePathFolder.EDIServiceEventPara,
                new string[] { execEDIEventNo.Substring(0, 8), execEDIEventNo });
            model.EventPara = Common.FileReadStream(eventParaFilePath);

            return View(model);
        }
    }
}