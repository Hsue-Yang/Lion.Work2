// 新增日期：2016-12-16
// 新增人員：廖先駿
// 新增內容：
// ---------------------------------------------------

using System;
using LionTech.Entity;
using LionTech.Entity.ERP.LineBotService;

namespace ERPAPI.Models.LineBotService
{
    public class LeaveModel : LineBotServiceModel
    {
        #region - Definitions -
        public class APIParaData
        {
            public string SysID { get; set; }
            public string LineID { get; set; }
            public string ReceiverID { get; set; }
        }
        #endregion

        #region - Constructor -
        public LeaveModel()
        {
            _entity = new EntityLeave(ConnectionStringSERP, ProviderNameSERP);
        }
        #endregion

        #region - API Property -
        public EntityLeave.LineReceiverInfo LineReceiverInfo { get; private set; }

        //[AllowHtml]
        public string APIPara { get; set; }

        public APIParaData APIData { get; set; }
        #endregion

        #region - Private -
        private readonly EntityLeave _entity;
        #endregion

        #region - 取得Line接收者資訊 -
        /// <summary>
        /// 取得Line接收者資訊
        /// </summary>
        /// <returns></returns>
        public bool GetLineReceiverInfoList()
        {
            try
            {
                LineReceiverInfo = _entity.SelectLineReceiverInfo(
                    new EntityLeave.LineReceiverInfoPara
                    {
                        SysID = new DBVarChar(ClientSysID),
                        LineID = new DBVarChar(APIData.LineID),
                        ReceiverID = new DBVarChar(APIData.ReceiverID)
                    });

                return LineReceiverInfo != null;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return false;
        }
        #endregion

        #region - 更新Line好友設定明細 -
        /// <summary>
        /// 更新Line好友設定明細
        /// </summary>
        public bool UpdateLineBotReceiverDetail()
        {
            try
            {
                EntityLeave.LineBotReceiverPara para = new EntityLeave.LineBotReceiverPara
                {
                    SysID = new DBVarChar(ClientSysID),
                    LineID = new DBVarChar(APIData.LineID),
                    ReceiverID = LineReceiverInfo.ReceiverID
                };

                return _entity.UpdateLineBotReceiverDetail(para) == EntityLeave.EnumUpdateLineBotReceiverResult.Success;
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