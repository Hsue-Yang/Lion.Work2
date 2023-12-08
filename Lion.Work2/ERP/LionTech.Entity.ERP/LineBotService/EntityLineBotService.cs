using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LionTech.Entity.ERP.LineBotService
{
    public class EntityLineBotService : DBEntity
    {
#if !NET461
        public EntityLineBotService(string connectionName)
            : base(connectionName)
        {
        }
#endif
        public EntityLineBotService(string connectionString, string providerName)
            : base(connectionString, providerName)
        {
        }

        #region - 查詢Line用戶端 -
        public class LineClinetPara
        {
            public enum ParaField
            {
                SYS_ID,
                LINE_ID
            }

            public DBVarChar SysID;
            public DBVarChar LineID;
        }

        public class LineClinet : DBTableRow
        {
            public DBVarChar ChannelAccessToken;
        }

        /// <summary>
        /// 查詢Line用戶端
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public LineClinet SelectLineClinet(LineClinetPara para)
        {
            var dbParameters = new List<DBParameter>();
            var commandText = new StringBuilder(string.Join(Environment.NewLine,
                new object[]
                {
                    "SELECT CHANNEL_ACCESS_TOKEN AS ChannelAccessToken",
                    "  FROM SYS_SYSTEM_LINE",
                    " WHERE SYS_ID = {SYS_ID}",
                    "   AND LINE_ID = {LINE_ID}",
                    "   AND IS_DISABLE = 'N';"
                }));

            dbParameters.Add(new DBParameter { Name = LineClinetPara.ParaField.SYS_ID, Value = para.SysID });
            dbParameters.Add(new DBParameter { Name = LineClinetPara.ParaField.LINE_ID, Value = para.LineID });
            return GetEntityList<LineClinet>(commandText.ToString(), dbParameters).SingleOrDefault();
        }
        #endregion
    }
}