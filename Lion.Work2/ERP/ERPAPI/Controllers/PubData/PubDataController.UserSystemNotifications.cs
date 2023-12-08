using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using ERPAPI.Models.PubData;
using LionTech.Utility;

namespace ERPAPI.Controllers.PubData
{
    //public partial class PubDataController
    //{
    //    [HttpGet]
    //    [AuthorizationActionFilter]
    //    public string UserSystemNotificationsInsertEvent(UserSystemNotificationsModel model)
    //    {
    //        if (AuthState.IsAuthorized == false)
    //        {
    //            return string.Empty;
    //        }

    //        string apiNo = model.ExecAPIClientBegin(model.ClientSysID, model.ClientUserID, HttpUtility.UrlDecode(HostUrl()), base.ClientIPAddress());
            
    //        try
    //        {
    //            JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
    //            model.APIData = jsonConvert.Deserialize<UserSystemNotificationsModel.APIParaData>(model.APIPara);

    //            if (model.GetInsertUserSystemNotifications(apiNo))
    //            {
    //                model.ExecAPIClientEnd(apiNo, apiNo);
    //                return apiNo;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            OnException(ex);
    //        }

    //        model.ExecAPIClientEnd(apiNo, string.Empty);
    //        return string.Empty;
    //    }

    //    [HttpGet]
    //    [AuthorizationActionFilter]
    //    public JsonResult<> UserSystemNotificationsSelectEvent(UserSystemNotificationsModel model)
    //    {
    //        if (AuthState.IsAuthorized == false)
    //        {
    //            return Json(null, JsonRequestBehavior.AllowGet);
    //        }

    //        try
    //        {
    //            JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
    //            model.APIData = jsonConvert.Deserialize<UserSystemNotificationsModel.APIParaData>(model.APIPara);

    //            if (model.GetUserSystemNotifications())
    //            {
    //                return Json((from s in model.SysUserSystemNotificationsList
    //                             select new
    //                                    {
    //                                        NoticeDateTime = s.NoticeDT.GetFormattedValue(Common.EnumDateTimeFormatted.FullDateTimeNumber),
    //                                        Content = s.NoticeContent.GetValue(),
    //                                        URL = s.NoticeURL.GetValue(),
    //                                        IsRead = s.IsRead.GetValue()
    //                                    }), JsonRequestBehavior.AllowGet);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            OnException(ex);
    //        }

    //        return Json(null, JsonRequestBehavior.AllowGet);
    //    }

    //    [HttpGet]
    //    [AuthorizationActionFilter]
    //    public JsonResult UserSystemNotificationsUnReadCountSelectEvent(UserSystemNotificationsModel model)
    //    {
    //        if (AuthState.IsAuthorized == false)
    //        {
    //            return Json(null, JsonRequestBehavior.AllowGet);
    //        }

    //        try
    //        {
    //            JavaScriptSerializer jsonConvert = new JavaScriptSerializer();
    //            model.APIData = jsonConvert.Deserialize<UserSystemNotificationsModel.APIParaData>(model.APIPara);

    //            if (model.GetUserSystemNotificationsUnReadCount())
    //            {
    //                return Json(model.UnReadCount, JsonRequestBehavior.AllowGet);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            OnException(ex);
    //        }

    //        return Json(null, JsonRequestBehavior.AllowGet);
    //    }
    //}
}
