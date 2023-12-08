using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LionTech.Entity.ERP;
using LionTech.Utility;
using LionTech.Utility.ERP;

namespace ERPAP.Models.Sys
{
    public class LineBotReceiverModel : SysModel
    {
        #region - Property -
        public string SysID { get; set; }

        public string SysNMID { get; set; }

        [StringLength(20)]
        public string LineID { get; set; }

        public string LineNMID { get; set; }

        public string ReceiverID { get; set; }

        public string LineReceiverNM { get; set; }

        public List<SystemLineBotReceiver> LineBotReceiverList { get; private set; }

        private Dictionary<string, string> _sourceTypeDic;

        public Dictionary<string, string> SourceTypeDic
        {
            get
            {
                if (_sourceTypeDic == null &&
                    CMCodeDictionary != null)
                {
                    _sourceTypeDic = (from s in CMCodeDictionary[Entity_BaseAP.EnumCMCodeKind.LineReceiverSourceType].Cast<Entity_BaseAP.CMCode>()
                                      select new
                                      {
                                          SourceType = s.CodeID.GetValue(),
                                          SourceTypeNM = s.CodeNM.GetValue()
                                      }).ToDictionary(k => k.SourceType, v => v.SourceTypeNM);
                }

                return _sourceTypeDic;
            }
        }
        #endregion

        #region - 取得Line名稱 -
        /// <summary>
        /// 取得Line名稱
        /// </summary>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> FormInitialize(string userID, EnumCultureID cultureID)
        {
            try
            {
                var sysSystemMain = GetSysSystemMain(SysID, cultureID);

                if (sysSystemMain != null)
                {
                    SysNMID = sysSystemMain.SysNMID;

                    string apiUrl = API.SystemLineBot.QuerySystemLineBotIDList(userID, SysID, cultureID.ToString().ToUpper());
                    string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                    var responseObj = Common.GetJsonDeserializeAnonymousType(response, new { lineBotIDList = (List<LineBotID>)null });

                    if (responseObj != null)
                    {
                        LineNMID = responseObj.lineBotIDList.Where(w => w.LineID == LineID).Select(s => s.LineNMID).FirstOrDefault();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 取得LionBot好友清單 -
        /// <summary>
        /// 取得LionBot好友清單
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="cultureID"></param>
        /// <returns></returns>
        public async Task<bool> GetLineBotReceiverList(int pageSize, string userID, EnumCultureID cultureID)
        {
            try
            {
                string apiUrl = API.SystemLineBotReceiver.QuerySystemLineBotReceiver(userID, SysID, LineID, cultureID.ToString().ToUpper(), LineReceiverNM, PageIndex, pageSize);
                string response = await Common.HttpWebRequestGetResponseStringAsync(apiUrl, AppSettings.APITimeOut);

                var responseObj = new
                {
                    RowCount = 0,
                    SystemLineReceiverList = (List<SystemLineBotReceiver>)null
                };

                responseObj = Common.GetJsonDeserializeAnonymousType(response, responseObj);

                if (responseObj != null)
                {
                    RowCount = responseObj.RowCount;
                    LineBotReceiverList = responseObj.SystemLineReceiverList;

                    SetPageCount();
                }
                return true;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion
    }
}