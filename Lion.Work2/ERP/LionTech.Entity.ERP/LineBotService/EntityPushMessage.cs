using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.LineBotService
{
    public class EntityPushMessage : EntityLineBotService
    {
        public EntityPushMessage(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 查詢接收者 -
        public class LineReceiverPara
        {
            public enum ParaField
            {
                SYS_ID,
                LINE_ID,
                LINE_RECEIVER_ID
            }

            public DBVarChar SysID;
            public DBVarChar LineID;
            public List<DBVarChar> LineReceiverIDList;
        }

        public class LineReceiver : DBTableRow
        {
            public DBVarChar ReceiverID;
        }

        /// <summary>
        /// 查詢接收者
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<LineReceiver> SelectLineReceiverList(LineReceiverPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT RECEIVER_ID AS ReceiverID",
                    "  FROM SYS_SYSTEM_LINE_RECEIVER",
                    " WHERE SYS_ID = {SYS_ID}",
                    "   AND LINE_ID = {LINE_ID}",
                    "   AND LINE_RECEIVER_ID IN ({LINE_RECEIVER_ID})",
                    "   AND IS_DISABLE = 'N';"
                }));

            dbParameters.Add(new DBParameter { Name = LineReceiverPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LineReceiverPara.ParaField.LINE_ID, Value = para.LineID });
            dbParameters.Add(new DBParameter { Name = LineReceiverPara.ParaField.LINE_RECEIVER_ID, Value = para.LineReceiverIDList });
            return GetEntityList<LineReceiver>(commandText.ToString(), dbParameters);
        }
        #endregion
    }
}