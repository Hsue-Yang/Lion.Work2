// 新增日期：2017-07-24
// 新增人員：王汶智
// 新增內容：
// ---------------------------------------------------

using System;
using LionTech.APIService.SMS;
using LionTech.Entity;
using LionTech.Entity.ERP.SMSService;
using LionTech.Utility;

namespace ERPAPI.Models.SMSService
{
    public class QueryModel : SMSServiceModel
    {
        #region - Constructor -
        public QueryModel()
        {
            _entity = new EntityQuery(ConnectionStringERP, ProviderNameERP);
        }
        #endregion

        #region - Property -
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public string SMSYear { get; set; }
        public int SMSSeq { get; set; }
        #endregion

        #region - Private -
        private readonly EntityQuery _entity;
        #endregion

        public SMSMessage GetSMSMessage()
        {
            try
            {
                var sms = _entity.SelectSMSDetail(new EntitySMSService.SMSDetailPara
                {
                    SMSYear = new DBChar(SMSYear),
                    SMSSeq = new DBInt(SMSSeq)
                });

                if (sms != null)
                {
                    var result = Common.GetJsonDeserializeObject<SMSMessage>(sms.SerializeToJson());
                    result.StateDesc = _GetSMSStateDesc(result.State);
                    return result;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return null;
        }

        private static string _GetSMSStateDesc(string sts)
        {
            string result;

            switch (sts)
            {
                case "":
                    result = "未發送";
                    break;
                case "0":
                case "1":
                    result = "已送達業者等待回應";
                    break;
                case "F":
                    result = "完成";
                    break;
                case "V":
                    result = "取消";
                    break;
                case "X":
                    result = "失敗";
                    break;
                case "101":
                    result = "手機端因素未能送達";
                    break;
                case "102":
                case "104":
                case "105":
                case "106":
                    result = "電信終端設備異常未能送達";
                    break;
                case "103":
                    result = "無此手機號碼";
                    break;
                case "107":
                    result = "逾時收訊";
                    break;
                case "108":
                    result = "語音簡訊發送失敗";
                    break;
                case "300":
                    result = "預約簡訊";
                    break;
                case "301":
                    result = "無額度(或額度不足) 無法發送";
                    break;
                case "303":
                    result = "取消簡訊";
                    break;
                case "500":
                    result = "未開通國際簡訊";
                    break;
                case "900":
                    result = "測試模式";
                    break;
                case "-3":
                    result = "無效門號";
                    break;
                case "-4":
                    result = "格式錯誤或預計發送時間已過去24小時以上";
                    break;
                case "4":
                    result = "已送達手機";
                    break;
                case "5":
                    result = "內容有錯誤";
                    break;
                case "6":
                    result = "門號有錯誤";
                    break;
                case "7":
                    result = "簡訊已停用";
                    break;
                case "8":
                    result = "逾時無送達";
                    break;
                case "9":
                    result = "預約已取消";
                    break;
                default:
                    result = "已送達業者";
                    break;
            }
            return result;
        }
    }
}