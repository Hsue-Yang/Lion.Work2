using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using LionTech.APIService.Message;
using LionTech.Entity;
using LionTech.Entity.ERP.ERPPubData;

namespace ERPAPI.Models.ERPPubData
{
    public class UserMessageModel : ERPPubDataModel
    {
        #region - Definitions -
        private enum EnumMDateTimeZone
        {
            TW
        }

        public class APIParaData
        {
            public string UserID { get; set; }
        }
        #endregion

        #region API Property
        public string ClientSysID { get; set; }
        public string ClientUserID { get; set; }
        public string APIPara { get; set; }
        public APIParaData APIData { get; set; }

        public List<EntityUserMessage.UserMessageReturn> EntityUserMessageReturnList
        {
            get { return _entityUserMessageReturnList; }
        }
        #endregion

        #region - Private -
        private string _entityUserMessageReturnListName = "UserMessages";
        private List<EntityUserMessage.UserMessageReturn> _entityUserMessageReturnList;
        #endregion
        
        public bool GetUserMessageReturnList()
        {
            try
            {
                EntityUserMessage.UserMessagePara para = new EntityUserMessage.UserMessagePara
                {
                    UserID = new DBVarChar(string.IsNullOrWhiteSpace(APIData.UserID) ? null : APIData.UserID)
                };

                var entity = new EntityUserMessage(ConnectionStringERP, ProviderNameERP);
                entity.ConnectionTimeout = 3;

                _entityUserMessageReturnList = entity.SelectUserMessageList(para);

                if (_entityUserMessageReturnList == null)
                {
                    _entityUserMessageReturnList = new List<EntityUserMessage.UserMessageReturn> {new EntityUserMessage.UserMessageReturn()};
                }
                return true;
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == -2)
                {
                    // timeout number
                    return false;
                }

                OnException(sqlex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }

        public string GetUserMessageReturnJsonString()
        {
            if (_entityUserMessageReturnList != null)
            {
                return DBEntity.DBTableRow.GetListSerializeToJson(_entityUserMessageReturnListName, _entityUserMessageReturnList);
            }
            return null;
        }

        #region - 新增使用者留言 -
        public bool AddUserMessage(Message message)
        {
            try
            {
                EntityUserMessage.MessagePara msgPara = new EntityUserMessage.MessagePara
                {
                    MsgStfn = new DBVarChar(message.MSG_STFN),
                    MsgYear = new DBChar(message.MSG_YEAR ?? string.Empty),
                    MsgOrdr = new DBInt(message.MSG_ORDR),
                    MsgSys = new DBBit(message.MSG_SYS),
                    MsgResponse = new DBBit(message.MSG_SYS == false),
                    MsgTimeOut = new DBBit(message.MSG_TIMEOUT),
                    MsgOrdrKind = new DBBit(string.IsNullOrWhiteSpace(message.MSG_YEAR) == false && message.MSG_ORDR != 0),
                    MsgProd = new DBVarChar(message.MSG_PROD),
                    MsgProdcomp = new DBChar(message.MSG_PRODCOMP),
                    MsgHDate = new DBChar(message.MSG_HDATE),
                    MsgMessage = new DBNVarChar(message.MSG_MESSAGE),
                    MsgURL = new DBVarChar(message.MSG_URL),
                    MsgMStfn = new DBVarChar(message.MSG_MSTFN),
                    MsgSts = new DBBit(false),
                    MsgMDateTimeZone = new DBVarChar(string.IsNullOrWhiteSpace(message.MSG_MDATE_TIMEZONE) ? EnumMDateTimeZone.TW.ToString() : message.MSG_MDATE_TIMEZONE),
                    MsgMDate = new DBDateTime(message.MSG_MDATE)
                };

                return new EntityUserMessage(ConnectionStringERP, ProviderNameERP)
                    .AddMessage(msgPara) == EntityUserMessage.EnumAddMessageResult.Success;
            }
            catch (Exception ex)
            {
                // 大部分都是pk會重複不紀錄錯誤訊息
            }

            return false;
        }
        #endregion
    }
}
